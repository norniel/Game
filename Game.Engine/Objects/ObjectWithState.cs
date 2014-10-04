using System;
using System.Collections.Generic;
using Game.Engine.Interfaces;

namespace Game.Engine.Objects
{
    abstract class ObjectWithState: FixedObject, IComparable<ObjectWithState>
    {
        // todo : object with state
        // to switch state objects are added to quiue in appropriate game tick
        // when appropriate tick is now - state of objects is changing
        // properties of object within one state is calculated when object interacts with hero
        // when state of object changes not by shedule - StateQueueManager searches for object and remove from queue and place it in another appropriate queue
        // game ticks to next state should be calculated with random distribution

        // todo : maybe rewrite with empty ctor and virtual methods for state registration
        public ObjectWithState(List<ObjectStateInfo> objectStateQueue, bool isCircling)
        {
            _objectStateQueue = objectStateQueue;
            _isCircling = isCircling;
        }

        public int NextStateTick { get; set; }

        public readonly List<ObjectStateInfo> _objectStateQueue;
        public readonly bool _isCircling;

        public virtual void ChangeState()
        {

        }

        public int CompareTo(ObjectWithState other)
        {
            if (NextStateTick.CompareTo(other.NextStateTick) != 0)
            {
                return NextStateTick.CompareTo(other.NextStateTick);
            }

            // todo important!!!!!! replace with id or rewrite GetHashCode!!!!
            return this.GetHashCode().CompareTo(other.GetHashCode());
        }

        public class ObjectStateInfo
        {
            private readonly int TickCount;
            private readonly int Distribution;

            private readonly IObjectState ObjectState;

            public ObjectStateInfo(IObjectState objectState, int tickCount, int distrubution)
            {
                ObjectState = objectState;
                TickCount = tickCount;
                Distribution = distrubution;
            }
        }
    }
}
