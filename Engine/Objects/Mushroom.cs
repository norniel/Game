namespace Engine.Objects
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using ObjectStates;

    internal class Mushroom : FixedObject, ICloneable, IEatable, IRoastable
    {
        private ObjectWithState ObjectWithState { get; set; }

        public override int Weight {get { return 2; }}
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
            this.Properties = new HashSet<Property>
            {
               Property.Pickable,
               Property.Regrowable,
               Property.Eatable,
               Property.Roastable
            };
        }

        public override string Name
        {
            get { return "Burovik"; }
        }


        public void OnLastStateFinished()
        {
            this.RemoveFromContainer();
        }

        public object Clone()
        {
            return new Mushroom();
        }

        public override uint GetDrawingCode()
        {
            if (ObjectWithState.CurrentState is Growing)
                return 0x10001900;

            return this.Id;
        }

        public int Poisoness
        {
            get
            {
                return 0;
            }
        }

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
