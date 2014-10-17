using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Game.Engine.States
{
    class Wondering:Moving
    {
        private readonly Size _viewPort; 
        public Wondering(MobileObject mobileObject, Size viewPort) : base(mobileObject, null)
        {
            _viewPort = viewPort;
        }

        protected override void Initialize()
        {
            int count = 0;
            var positionCell = Map.PointToCell(_mobileObject.Position);
            var rigthBorder = (positionCell.X + 1 == Game.Map.GetSize().Width) ? 0 : 1;
            var bottomBorder = (positionCell.X + 1 == Game.Map.GetSize().Height) ? 0 : 1;
            var neighbourList = new List<Point>();

            for (int i = positionCell.X == 0 ? 0 : -1; i <= rigthBorder; i++)
            {
                for (int j = positionCell.Y == 0 ? 0 : -1; j <= bottomBorder; j++)
                {
                    if(i == 0 && j == 0)
                        continue;

                    neighbourList.Add(new Point(positionCell.X + i, positionCell.Y + j));
                }
            }

            _destination = _mobileObject.Position;

            while (neighbourList.Any())
            {
                var p = Game.Random.Next(neighbourList.Count);

                var obj = Game.Map.GetObjectFromCell(neighbourList[p]);
                if (obj != null && !obj.IsPassable)
                {
                    neighbourList.RemoveAt(p);
                    continue;
                }

                _destination = Map.CellToPoint(neighbourList[p]);
                break;
            }


            base.Initialize();
        }
    }
}
