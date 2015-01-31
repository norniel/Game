using System;
using System.Collections.Generic;
using Game.Engine.Interfaces;
using Game.Engine.ObjectStates;

namespace Game.Engine.Objects
{
    internal class Plant: FixedObject, IBurnable, ICloneable
    {
        private ObjectWithState ObjectWithState { get; set; }

        public Plant()
        {
            IsPassable = true;

            Size = new Size(1, 1);

            Id = 0x00001100;

            ObjectWithState =
                new ObjectWithState(
                    new List<IObjectState>
                    {
                        new Growing {TickCount = 300, Distribution = 30},
                        new Staying() {TickCount = 1000, Distribution = 100},
                        new Drying() {TickCount = 300, Distribution = 30}
                    }, 
                    false, 
                    OnLastStateFinished);
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
            get
            {
                if (ObjectWithState.CurrentState is Growing)
                {
                    var tToNext = ObjectWithState.TicksToNextState;
                    var tCount = ObjectWithState.CurrentState.TickCount;

                    return tCount > 0 ? (tToNext/tCount)*100 : 0;
                }

                return 100;
            }
        }

        public void OnLastStateFinished()
        {
            this.RemoveFromContainer();
        }

        public object Clone()
        {
            return new Plant();
        }

        public override uint GetDrawingCode()
        {
            if (ObjectWithState.CurrentState is Growing)
                return 0x10001100;

            if (ObjectWithState.CurrentState is Drying)
                return 0x20001100;

            return this.Id;
        }
    }
}
