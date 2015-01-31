using System;
using System.Collections.Generic;
using Game.Engine.Interfaces;
using Game.Engine.ObjectStates;
using Game.Engine.States;

namespace Game.Engine.Objects
{
    class Dikabryozik: MobileObject
    {
        private ObjectWithState ObjectWithState { get; set; }
        public Dikabryozik(Point position)
        {
            IsPassable = false;

            Size = new Size(1, 1);

            Id = 0x00018000;

            Speed = 1;

            ViewSight = new Size(6, 6);
            Position = position;
            this.StateEvent.FireEvent();

            ObjectWithState = new ObjectWithState(
                new List<IObjectState>
                    {
                        new Staying() {TickCount = 1000, Distribution = 100, Eternal = false},
                        new Hungry() {TickCount = 300, Distribution = 30, Eternal = true}
                    },
                    false, null, StartLookingForFood);
        }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
            };
        }

        public override string Name
        {
            get { return "Dikabryozik"; }
        }

        public override IState GetNextState()
        {
            var dice = Game.Random.Next(3);
            if (dice == 2)
            {
                return new Resting(this);
            }

            return new Wondering(this, this.ViewSight);
        }

        public override uint GetDrawingCode()
        {
            return base.GetDrawingCode() + 90+(uint)this.Angle;
        }

        public override bool CheckForUnExpected()
        {
            for (int i = 0; i < ViewSight.Width; i++)
            {
                for (int j = 0; j < ViewSight.Height; j++)
                {
                    if(i == 0 && j == 0)
                        continue;


                }
            }
            return true;
        }

        public void StartLookingForFood()
        {
          /*  for (int i = 0; i < ViewSight.Width; i++)
            {
                for (int j = 0; j < ViewSight.Height; j++)
                {
                    if (i == 0 && j == 0)
                        continue;


                }
            }
            return true;*/
        }
    }
}
