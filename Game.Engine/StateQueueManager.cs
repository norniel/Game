using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Game.Engine.Objects;
using Wintellect.PowerCollections;

namespace Game.Engine
{
    internal class StateQueueManager : IObserver<long>
    {
        private int _currentTick = 0;
        private OrderedBag< ObjectWithState> _queue = new OrderedBag<ObjectWithState>();
        public void OnNext(long value)
        {
            // should be done with locking
            if (!_queue.Any())
            {
                _currentTick++;
                return;
            }

            // todo maybe write use tasks here
            while (_queue.GetFirst().NextStateTick <= _currentTick)
            {
                _queue.GetFirst().ChangeState();
                _queue.RemoveFirst();
            }
            _currentTick++;
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void AddObjectToQueue(int nextStateInterval, ObjectWithState objectWithState)
        {
            // should be done with locking
            _queue.Remove(objectWithState);
            objectWithState.NextStateTick = _currentTick + nextStateInterval;
            _queue.Add(objectWithState);
        }
    }
 
}
