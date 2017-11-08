using System.Collections.Generic;
using Engine.Interfaces;
using Engine.ObjectStates;

namespace Engine.Objects
{
    internal class Fire : FixedObject, IBurning
    {
        private ObjectWithState ObjectWithState { get; set; }

        public Fire()
        {
            IsPassable = false;

            Size = new Size(1, 1);

            Id = 0x00000600;

            ObjectWithState =
                new ObjectWithState(
                    new List<IObjectState>
                    {
                        new Firing {TickCount = 300, Distribution = 10, Eternal = false},
                        new Attenuating {TickCount = 150, Distribution = 10, Eternal = false}
                    }, 
                    false,
                    OnLastStateFinished);
        }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {Property.Burning};
        }

        public override string Name => "Fire";


        public int TimeOfBurning {
            get
            {
                if (this.ObjectWithState.CurrentState is Firing)
                {
                    return this.ObjectWithState.TicksToNextState;
                }

                return 0;
            }
            set => this.ObjectWithState.ChangeState(0, value);
        }

        public int LightRadius {
            get
            {
                if (this.ObjectWithState.CurrentState is Attenuating)
                    return 1;

                return 3;
            }
        }

        public override uint GetDrawingCode()
        {
            if (this.ObjectWithState.CurrentState is Attenuating)
                return 0x00001500;

            return this.Id;
        }

        public void OnLastStateFinished()
        {
            if (this.RemoveFromContainer != null)
            {
                this.RemoveFromContainer();
            }
        }
    }
}
