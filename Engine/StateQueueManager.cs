using System;
using System.Linq;
using Engine.Objects;
using Wintellect.PowerCollections;


namespace Engine
{
    internal class StateQueueManager : IObserver<long>
    {
        public int CurrentTick { get; set; }
        private OrderedBag< ObjectWithState> _queue = new OrderedBag<ObjectWithState>();

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
            var first = _queue.GetFirst();

            while (_queue.Any() && (first.NextStateTick <= CurrentTick))
            {
                if(first.CurrentState == null || !first.CurrentState.Eternal)
                {
                    _queue.RemoveFirst();
                    first.NextState();
                }
                else 
                {
                    first.NextStateTick += 1;
                }

                first = _queue.GetFirst();
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
            _queue.Remove(objectWithState);
        }

        public void MoveObjectInQueue(int nextStateInterval, int distribution, ObjectWithState objectWithState)
        {
            // should be done with locking
            _queue.Remove(objectWithState);
            AddObjectToQueue(nextStateInterval, distribution, objectWithState);
        }

        public void AddObjectToQueue(int nextStateInterval, int distribution, ObjectWithState objectWithState)
        {
            // should be done with locking
            objectWithState.NextStateTick = CurrentTick + nextStateInterval + 2 * distribution - (int)(distribution * Random.NextDouble());
            _queue.Add(objectWithState);
        }
    }
 
}
