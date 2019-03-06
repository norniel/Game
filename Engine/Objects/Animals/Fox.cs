using System.Collections.Generic;
using System.Linq;
using Engine.States;
using Engine.Tools;

namespace Engine.Objects.Animals
{
    class Fox :Animal
    {
        public Fox( Point position) 
            : base(false, new Size(1,1), 0x00020000, 50, "Fox", 4, position)
        {
        }

        protected override bool IsCellEatable(PointWithDistance p)
        {
            throw new System.NotImplementedException();
        }

        public override EaterType EaterType => EaterType.Carnivorous;

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>();
        }

        public override uint GetDrawingCode()
        {
            return 0x00020000 + 90 + (uint)Angle;
        }

        protected override bool LookForFood()
        {
            var destinationObject = VisibleCells.Select(vCell =>
            {
                var t =  Game.Map.GetMobileObject(vCell);
                return t;
            }).FirstOrDefault(mo =>
            {
                return mo != null && mo.Name == "Hare";
            });

            if (destinationObject != null)
            {
                _stateQueue.Enqueue(new Pursuing(this, destinationObject));
                _stateQueue.Enqueue(new Resting(this));

                return true;
            }

            return false;
        }
    }
}
