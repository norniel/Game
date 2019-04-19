using System.Collections.Generic;
using System.Linq;
using Engine.Resources;
using Engine.States;
using Engine.Tools;
using Unity;

namespace Engine.Objects.Animals
{
    class Hare: Animal
    {
        [Dependency] 
        public ObjectsFactory ObjectsFactory { get; set; }

        public Hare(Point position) 
            : base(false, new Size(1,1), 0x00019000, 60, "Hare", 4, position)
        {
        }

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>();
        }

        public override uint GetDrawingCode()
        {
            return 0x00019000;
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

            return !FleeEnemies();
        }

        public override IEnumerable<MobileObject> GetEnemies()
        {
            return VisibleCells.SelectMany(c => Game.Map.GetMobileObjects().Where(mb => mb.PositionCell == c).Where(mb => mb.Name == "Fox"))
                .Union(VisibleCells.Where(vc => Hero.PositionCell == vc).Select(vc => Hero));
        }
        public override bool IsMoving => State is Moving || State is Fleeing || State is Pursuing || State is MovingToObject;


        public override void Die()
        {
            base.Die();

            _stateQueue.Clear();
            Game.Map.RemoveMobileObject(this);
            Game.Map.SetObjectFromCell(PositionCell,  Game.Factory.Produce("Dead hare") as FixedObject);
            _disposables.ForEach(d => d.Dispose());

            var position = PositionCell;
            
            Game.PlannedQueueManager.AddObjectToQueue(new PlannedEvent(() =>
            {
                var nearCell = Game.Map.GetRandomNearEmptyPoint(position, 3);
                var hare = new Hare(Map.CellToPoint(nearCell ?? position));
                hare.Hero = Hero; //!!!!!!! todo correct that!!!
                Game.Map.AddMobileObject(hare);

                return true;
            }));
        }
    }
}
