using System;
using System.Threading;

namespace Engine.Objects
{
    public class ObjectWithStateBase : IComparable<ObjectWithStateBase>
    {
        private static int _idCounter;
        protected int _id;
        public virtual int NextStateTick { get; set; }

        public ObjectWithStateBase()
        {
            _id = GenerateId();
        }

        private static int GenerateId()
        {
            return Interlocked.Increment(ref _idCounter);
        }

        public int CompareTo(ObjectWithStateBase other)
        {
            if (NextStateTick.CompareTo(other.NextStateTick) != 0)
            {
                return NextStateTick.CompareTo(other.NextStateTick);
            }

            return _id.CompareTo(other._id);
        }
    }
}