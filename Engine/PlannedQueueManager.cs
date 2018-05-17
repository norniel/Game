using System;
using System.Linq;
using Engine.Objects;
using Wintellect.PowerCollections;

namespace Engine
{
    internal class PlannedQueueManager : IObserver<long>
    {
        public int CurrentTick { get; set; }
        private OrderedBag<PlannedEvent> _queue = new OrderedBag<PlannedEvent>();

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(long value)
        {
            // should be done with locking
            if (!_queue.Any())
            {
                CurrentTick++;
                return;
            }
            
            var first = _queue.GetFirst();

            while (_queue.Any() && (first.NextStateTick <= CurrentTick))
            {
                RemoveObjectFromQueue(first);
                var result = first.Act();

                if (!result)
                {
                    AddObjectToQueue(first);
                }

                if(_queue.Any())
                    first = _queue.GetFirst();
            }

            CurrentTick++;
        }

        public void RemoveObjectFromQueue(PlannedEvent plannedEvent)
        {
            _queue.Remove(plannedEvent);
        }

        public void AddObjectToQueue(PlannedEvent plannedEvent)
        {
            plannedEvent.NextStateTick = CurrentTick + 100 - (int)(50 * Game.Random.NextDouble());
            _queue.Add(plannedEvent);
            
        }
    }
}
