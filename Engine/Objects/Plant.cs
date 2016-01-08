using System;
using System.Collections.Generic;
using Engine.Interfaces;
using Engine.ObjectStates;
using Engine.Resources;

namespace Engine.Objects
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
                        new Growing {TickCount = 300, Distribution = 50, Eternal = false},
                        new Staying() {TickCount = 1000, Distribution = 100, Eternal = false},
                        new Drying() {TickCount = 300, Distribution = 30, Eternal = false}
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
               Property.Regrowable,
               Property.NeedToBuildGrassBed
            };
        }

        public override string Name
        {
            get { return Resource.Plant; }
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
