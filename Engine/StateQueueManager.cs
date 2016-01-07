using System;
using System.Linq;
using Academy.Collections.Generic;
using Engine.Objects;


namespace Engine
{
    internal class StateQueueManager : IObserver<long>
    {
        public int CurrentTick { get; set; }
        private PriorityQueue< ObjectWithState> _queue = new PriorityQueue<ObjectWithState>();

        //todo - get through dependency/ make single random for all game needs
        private Random Random = new Random(1);
        public void OnNext(long value)
        {
            // should be done with locking
            if (!_queue.Any())
            {
                CurrentTick++;
                return;
            }

            // todo maybe write use tasks here
            var first = _queue.Peek();

            while (_queue.Any() && (first.NextStateTick <= CurrentTick))
            {
                if (first.CurrentState.Eternal)
                {
                    first.NextStateTick += 1;
                }
                else
                {
                    first.NextState();
                    _queue.Dequeue();
                }
            }
            CurrentTick++;
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void RemoveObjectFromQueue(ObjectWithState objectWithState)
        {
            // should be done with locking
            _queue = new PriorityQueue<ObjectWithState>(_queue.Except(new[] { objectWithState }));
        }

        public void MoveObjectInQueue(int nextStateInterval, int distribution, ObjectWithState objectWithState)
        {
            // should be done with locking
            _queue = new PriorityQueue<ObjectWithState>(_queue.Except(new[] { objectWithState }));
            AddObjectToQueue(nextStateInterval, distribution, objectWithState);
        }

        public void AddObjectToQueue(int nextStateInterval, int distribution, ObjectWithState objectWithState)
        {
            // should be done with locking
            objectWithState.NextStateTick = CurrentTick + nextStateInterval + 2 * distribution - (int)(distribution * Random.NextDouble());
            _queue.Enqueue(objectWithState);
        }
    }
 
}
