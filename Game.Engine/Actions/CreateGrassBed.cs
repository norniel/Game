using System;
using System.Collections.Generic;
using System.Linq;
using Game.Engine.Heros;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;
using Game.Engine.Objects.LargeObjects;

namespace Game.Engine.Actions
{
    class CreateGrassBed : IAction
    {
        private  bool _isInitialized = false;

        private GrassBed _grassBed = null;

        private List<Plant> _plants = null;

        public string Name
        {
            get { return "Build grass bed"; }
        }

        public string GetName(IEnumerable<GameObject> objects)
        {
            if (objects.Any(o => o is GrassBed))
            {
                return "Continue building";
            }

            return Name;
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.NeedToBuildGrassBed;
        }

        public bool Do(Hero hero, IEnumerable<GameObject> objects)
        {
            if (!_isInitialized)
            {
                Initialize(hero, objects);
                _isInitialized = true;
            }

            bool isOver = DoOnEachAction(hero);

            if (isOver)
            {
                _plants = null;
                _grassBed = null;
                _isInitialized = false;
            }

            return isOver;
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<List<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            var allObjects = objects.Union(hero.GetContainerItems()).Distinct();
            var grassBed = (GrassBed)allObjects.FirstOrDefault(gb => gb is GrassBed);
            if (grassBed != null && !grassBed.IsBuild)
            {
                var plants = allObjects.OfType<Plant>().Take(grassBed.CountLeftToBuild);
                if (plants.Any())
                {
                    yield return plants.OfType<GameObject>().Union(new[]{grassBed}).ToList();
                }
            }
            else if (grassBed == null)
            {
                var cell = Map.PointToCell(hero.Position);
                var mapSize = Game.Map.GetSize();

                if(cell.Y + 1 >= mapSize.Height)
                    yield break;

                var objectOnPlace = Game.Map.GetObjectFromCell(cell);
                var objectOnNextPlace = Game.Map.GetObjectFromCell(new Point(cell.X, cell.Y + 1));

                if (objectOnPlace != null || objectOnNextPlace != null)
                {
                    yield break;
                }

                var plants = allObjects.OfType<Plant>().Take(GrassBed.CountToBuild);
                if (plants.Any())
                {
                    yield return plants.OfType<GameObject>().ToList();
                }
            }
        }

        public double GetTiredness()
        {
            return 0.5;
        }

        protected void Initialize(Hero hero, IEnumerable<GameObject> objects)
        {
            _plants = objects.OfType<Plant>().ToList();

            _grassBed = (GrassBed) objects.FirstOrDefault(o => o is GrassBed);
        }

        private bool DoOnEachAction(Hero hero)
        {
            if (_grassBed == null)
            {
                _grassBed = new GrassBed();
                Game.Map.SetObjectFromDestination(hero.Position, _grassBed);
            }

            var plantsToUseCount = 2;
            for (int i = 0; i < plantsToUseCount && _plants.Any(); i++)
            {
                _plants[0].RemoveFromContainer();
                _plants.RemoveAt(0);
                
                _grassBed.CountLeftToBuild = _grassBed.CountLeftToBuild - 1;
            }

            return _grassBed.IsBuild || !_plants.Any();
        }
    }
}
