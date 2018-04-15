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
    internal class Mushroom : FixedObject, ICloneable
    {
        private ObjectWithState ObjectWithState { get; }

        public override int Weight => 2;

        public Mushroom()
        {
            IsPassable = true;

            Size = new Size(1, 1);

            Id = 0x00001900;

            ObjectWithState =
                new ObjectWithState(
                    new List<IObjectState>
                    {
                        new Growing {TickCount = 300, Distribution = 50, Eternal = false},
                        new Staying{TickCount = 1000, Distribution = 100, Eternal = false}
                    },
                    false,
                    OnLastStateFinished);
        }


        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>
            {
               Property.Pickable,
               Property.Regrowable,
               Property.Eatable,
               Property.Roastable
            };
        }

        public override void InitializeBehaviors()
        {
            base.InitializeBehaviors();
            Behaviors.Add(new RoastBehavior(new RoastedMushroom()));
            Behaviors.Add(new EatableBehavior(2));
        }

        public override string Name => Resource.Burovik;


        public void OnLastStateFinished()
        {
            RemoveFromContainer?.Invoke();
        }

        public object Clone()
        {
            return new Mushroom();
        }

        public override uint GetDrawingCode()
        {
            if (ObjectWithState.CurrentState is Growing)
                return 0x10001900;

            return Id;
        }
        
        public override double WeightDbl
        {
            get
            {
                if (ObjectWithState.CurrentState is Growing)
                    return 0.5;

                return 1;
            }
        }
    }
}
