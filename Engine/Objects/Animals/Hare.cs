using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using Engine.Resources;
using Engine.States;
using Engine.Tools;

namespace Engine.Objects.Animals
{
    class Hare: Animal
    {
        public Hare(Point position) 
            : base(false, new Size(1,1), 0x00019000, 50, "Hare", 4, position)
        {
        }

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>();
        }

        public override uint GetDrawingCode()
        {
            return 0x00019000 + 90 + (uint)Angle;
        }

        protected override bool IsCellEatable(PointWithDistance p)
        {
            var obj = Game.Map.GetObjectFromCell(p);
            return obj != null && obj.GetBaseName() == Resource.Plant;
        }

        public override EaterType EaterType => EaterType.Herbivorous;

        public override bool CheckForUnExpected()
        {
            if (State is Fleeing)
            {
                return true;
            }

            var enemiesExist = GetEnemies().Any();

            if (enemiesExist)
            {
                _stateQueue.Clear();
                _stateQueue.Enqueue(new Fleeing(this));
                StateEvent.FireEvent();

                return false;
            }

            return true;
        }

        public override IEnumerable<MobileObject> GetEnemies()
        {
            return VisibleCells.SelectMany(c => Game.Map.GetMobileObjects().Where(mb => mb.PositionCell == c).Where(mb => mb.Name == "Fox"));
        }

        public override void Die()
        {
            base.Die();

            _stateQueue.Clear();
            Game.Map.RemoveMobileObject(this);
            Game.Map.SetObjectFromCell(PositionCell,  Game.Factory.Produce("Dead hare") as FixedObject);
            _disposables.ForEach(d => d.Dispose());
        }
    }
}
