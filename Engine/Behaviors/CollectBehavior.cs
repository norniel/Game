using System;
using System.Collections.Generic;
using Engine.Interfaces;
using Engine.Objects;
using Engine.ObjectStates;

namespace Engine.Behaviors
{
    internal class CollectBehavior<T> :IBehavior where T : GameObject
    {
        private readonly bool _isRegrowable;
        private int _currentCount;
        public ObjectWithState ObjectWithState { get; }

        public int PerCollectCount { get; private set; }

        public int TotalCount { get; private set; }

        public int CurrentCount
        {
            get { return _currentCount; }
            set
            {
                if (_isRegrowable && value < TotalCount && ObjectWithState != null && ObjectWithState?.CurrentState == null)
                {
                    ObjectWithState.ChangeState(0);
                }

                _currentCount = value;
            }
        }

        public string Name { get; set; }

        public GameObject GetSmth()
        {
            return Game.Factory.Produce(Name);
        }

        public CollectBehavior(string name, int perCollectCount, int totalCount, bool isRegrowable = true)
        {
            _isRegrowable = isRegrowable;
            Name = name;
            PerCollectCount = perCollectCount;
            TotalCount = totalCount;
            CurrentCount = totalCount;

            if (_isRegrowable)
            {
                ObjectWithState =
                new ObjectWithState(
                    new List<ObjectState>
                    {
                        new ObjectState(ObjectStates.ObjectStates.Growing, new ObjStateProperties{TickCount=DayNightCycle.HalfDayLength, Distribution= DayNightCycle.HalfDayLength, Eternal= false})
                    },
                    false,
                    OnLastStateFinished, null, true);
            }
        }

        public void OnLastStateFinished()
        {
            CurrentCount= Math.Min(CurrentCount + 1, TotalCount);
            if (CurrentCount < TotalCount)
            {
                ObjectWithState.ChangeState(0);
            }
        }
    }
}
