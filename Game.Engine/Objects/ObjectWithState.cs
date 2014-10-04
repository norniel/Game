using System;

namespace Game.Engine.Objects
{
    class ObjectWithState: FixedObject, IComparable<ObjectWithState>
    {
        // todo : object with state
        // to switch state objects are added to quiue in appropriate game tick
        // when appropriate tick is now - state of objects is changing
        // properties of object within one state is calculated when object interacts with hero
        // when state of object changes not by shedule - StateQueueManager searches for object and remove from queue and place it in another appropriate queue
        // game ticks to next state should be calculated with random distribution
        public virtual void ChangeState()
        {

        }

        public int NextStateTick { get; set; }
        public int CompareTo(ObjectWithState other)
        {
            if (NextStateTick.CompareTo(other.NextStateTick) != 0)
            {
                return NextStateTick.CompareTo(other.NextStateTick);
            }

            // todo important!!!!!! replace with id or rewrite GetHashCode!!!!
            return this.GetHashCode().CompareTo(other.GetHashCode());
        }
    }
}
