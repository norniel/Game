using System;
using System.Collections.Generic;
using Engine.Interfaces;
using Engine.ObjectStates;
using Engine.Resources;
using Engine.Tools;

namespace Engine.Objects
{
    [GenerateMap]
    internal class Mushroom : FixedObject, ICloneable, IEatable, IRoastable
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

        public int Poisoness => 0;

        public int Satiety
        {
            get
            {
                if (ObjectWithState.CurrentState is Growing)
                    return 1;

                return 2;
            }
        }

        public GameObject GetRoasted()
        {
            return new RoastedMushroom();
        }
    }
}
