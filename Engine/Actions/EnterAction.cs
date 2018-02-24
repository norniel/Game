using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Objects.LargeObjects;
using Engine.Resources;
using Engine.Tools;

namespace Engine.Actions
{
    internal class EnterAction : IAction
    {
        private Point innerPoint;

        public string Name => ActionsResource.Enter;

        public string GetName(IEnumerable<GameObject> objects)
        {
            return Name;
        }

        public Knowledges GetKnowledge()
        {
            return Knowledges.Nothing;
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.Enterable;
        }

        public IActionResult Do(Hero hero, IList<GameObject> objects)
        {
            var innerObject = objects.OfType<Wickiup>().First();
            Game.Map.SetInnerMap(innerObject._map, innerPoint);

            innerPoint = null;
            return new FinishedActionResult();
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            var objectsToEnter = objects.Where(o =>
            {
                var largeObj = o as LargeObjectInner;
                return largeObj == null || largeObj.IsBuild;
            }).ToList();

            return objectsToEnter.Any() ? new[] { objectsToEnter } : new List<GameObject>[] {};
        }

        public double GetTiredness()
        {
            return 0;
        }

        public Point GetDestination(Point destination, FixedObject destObject, Hero hero)
        {
            var cell = Map.PointToCell(destination);
            var obj = Game.Map.GetObjectFromCell(cell);

            var largeObjectOuter = obj as LargeObjectOuterAbstract;

            if(largeObjectOuter == null)
                return destination;

            var startingPoint = new Point(cell.X - largeObjectOuter.PlaceInObject.X, cell.Y - largeObjectOuter.PlaceInObject.Y);

            var innerObject = largeObjectOuter.InnerObject as Wickiup;
            var totDist = Int32.MaxValue;
            Point p = null;
            var heroPos = Map.PointToCell(hero.Position);

            for (int i = 0; i < (int)innerObject.Size.Width + 2; i++ )
            {
                for (int j = 0; j < (int) innerObject.Size.Height + 2; j++)
                {
                    if (i > 0 && i < (int) innerObject.Size.Width + 1 && j > 0 && j < (int) innerObject.Size.Height + 1)
                        continue;

                    var curPoint = new Point(i - 1, j - 1);
                    bool isPassable;

                    if (j == 0 || j == (int) innerObject.Size.Height + 1)
                    {
                        isPassable = innerObject.HorizontalBorder[i, j == 0 ? 0 : 1];
                    }
                    else
                    {
                        isPassable = innerObject.VerticalBorder[j - 1, i == 0 ? 0 : 1];
                    }

                    if (!isPassable)
                        continue;

                    int totdistX = Math.Abs(heroPos.X - startingPoint.X - curPoint.X);
                    int totdistY = Math.Abs(heroPos.Y - startingPoint.Y - curPoint.Y);

                    var curtotDist = totdistX*totdistX + totdistY*totdistY;

                    if (curtotDist < totDist)
                    {
                        totDist = curtotDist;
                        p = curPoint;
                    }
                }
            }

            var newCenterPoint = Map.CellToPoint(new Point(startingPoint.X + p.X, startingPoint.Y + p.Y));
            var x = newCenterPoint.X + Map.CellMeasure / 2 + (p.X > -1 && p.X < innerObject.Size.Width ? 0 :  p.X == -1 ? Map.CellMeasure / 2 + 2 : -Map.CellMeasure / 2 - 2);
            var y = newCenterPoint.Y + Map.CellMeasure / 2 + (p.Y > -1 && p.Y < innerObject.Size.Height ? 0 : p.Y == -1 ? Map.CellMeasure / 2 + 2 : -Map.CellMeasure / 2 - 2);

            innerPoint = startingPoint;

            return new Point(x, y);
        }
    }
}
