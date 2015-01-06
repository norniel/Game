using System;
using System.Collections.Generic;
using Game.Engine.Interfaces;
using Game.Engine.ObjectStates;

namespace Game.Engine.Objects
{
    internal class Plant: ObjectWithState, IBurnable, ICloneable
    {
        public Plant()
            : base(new List<IObjectState> { new Growing { TickCount = 300, Distribution = 10 }, new Staying() { TickCount = 1000, Distribution = 100 }, new Drying() { TickCount = 300, Distribution = 30 } }, false)
        {
            IsPassable = true;

            Size = new Size(1, 1);

            Id = 0x00001100;
        }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
               Property.Pickable,
               Property.NeedToCreateFire,
               Property.Regrowable
            };
        }

        public override string Name
        {
            get { return "Plant"; }
        }

        public int TimeOfBurning {
            get { return 100; }
        }

        public override void OnLastStateFinished()
        {
            this.RemoveFromContainer();
        }

        public object Clone()
        {
            return new Plant();
        }

        public override uint GetDrawingCode()
        {
            if (CurrentState is Growing)
                return 0x10001100;

            if (CurrentState is Drying)
                return 0x20001100;

            return this.Id;
        }
    }
}
