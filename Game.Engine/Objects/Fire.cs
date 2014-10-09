using System.Collections.Generic;
using Game.Engine.Interfaces;
using Game.Engine.ObjectStates;

namespace Game.Engine.Objects
{
    internal class Fire : ObjectWithState, IBurning
    {
        public Fire()
            :base(new List<IObjectState>{new Firing{TickCount = 300, Distribution = 10}, new Attenuating{TickCount = 150, Distribution = 10}}, false)
        {
            IsPassable = false;

            Size = new Size(1, 1);

            Id = 0x00000600;
        }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {Property.Burning};
        }

        public override string Name
        {
            get { return "Fire"; }
        }


        public int TimeOfBurning {
            get
            {
                if (this.CurrentState is Firing)
                {
                    return this.TicksToNextState;
                }

                return 0;
            }
            set
            {
                this._objectStateQueue[0].TickCount = value;
                this.ChangeState(0);
            }
        }

        public override uint GetDrawingCode()
        {
            if(CurrentState is Attenuating)
                return 0x00001500;

            return this.Id;
        }
    }
}
