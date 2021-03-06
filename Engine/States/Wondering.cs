﻿using System.Linq;
using Engine.Heros;

namespace Engine.States
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
            var neighbourList = Game.Map.GetNearestToPointList(_mobileObject.Position, 1);

            _destination = _mobileObject.Position;
            var positionCell = Map.PointToCell(_mobileObject.Position);
            neighbourList.Remove(positionCell);

            while (neighbourList.Any())
            {
                var p = Game.Random.Next(neighbourList.Count);

                var obj = _mobileObject is Hero ? Game.Map.GetHObjectFromCell(neighbourList[p]): Game.Map.GetObjectFromCell(neighbourList[p]);
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
