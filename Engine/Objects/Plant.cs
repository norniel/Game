using System;
using System.Collections.Generic;
using Engine.Behaviors;
using Engine.Interfaces;
using Engine.ObjectStates;
using Engine.Resources;
using Engine.Tools;

namespace Engine.Objects
{
    [GenerateMap]
    internal class Plant: FixedObject, ICloneable
    {
        private ObjectWithState ObjectWithState { get; }

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
                        new Staying {TickCount = 1000, Distribution = 100, Eternal = false},
                        new Drying {TickCount = 300, Distribution = 30, Eternal = false}
                    }, 
                    false, 
                    OnLastStateFinished);
        }

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>
            {
               Property.Pickable,
               Property.NeedToMakeFireWithWood,
               Property.Regrowable,
               Property.NeedToBuildGrassBed
            };
        }

        public override void InitializeBehaviors()
        {
            base.InitializeBehaviors();
            Behaviors.Add(new BurnableBehavior(300));
        }

        public override string Name => Resource.Plant;

        public override double WeightDbl
        {
            get
            {
                if (ObjectWithState.CurrentState is Growing)
                {
                    var tToNext = ObjectWithState.TicksToNextState;
                    var tCount = ObjectWithState.CurrentState.TickCount;

                    return tCount > 0 ? (tToNext/tCount) : 0;
                }

                return 1.0;
            }
        }

        public void OnLastStateFinished()
        {
            RemoveFromContainer?.Invoke();
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

            return Id;
        }
    }
}
