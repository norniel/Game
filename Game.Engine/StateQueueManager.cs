using System;
using System.Linq;
using Game.Engine.Objects;
using Wintellect.PowerCollections;

namespace Game.Engine
{
    internal class StateQueueManager : IObserver<long>
    {
        public int CurrentTick { get; set; }
        private OrderedBag< ObjectWithState> _queue = new OrderedBag<ObjectWithState>();
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
            while (_queue.Any() && (_queue.GetFirst().NextStateTick <= CurrentTick))
            {
                _queue.GetFirst().NextState();
                _queue.RemoveFirst();
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
