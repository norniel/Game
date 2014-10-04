using System.Collections.Generic;
using Game.Engine.ObjectStates;

namespace Game.Engine.Objects
{
    internal class Fire : ObjectWithState
    {
        public Fire()
            :base(new List<ObjectStateInfo>(){new ObjectStateInfo(new Firing(), 100, 10), new ObjectStateInfo(new Attenuating(), 50, 10)}, false)
        {
            IsPassable = false;

            Size = new Size(1, 1);

            Id = 0x00000600;
        }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {};
        }

        public override string Name
        {
            get { return "Fire"; }
        }
    }
}
