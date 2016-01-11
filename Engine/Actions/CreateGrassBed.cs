using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Objects.LargeObjects;
using Engine.Objects.LargeObjects.BuilderPlans;
using Engine.Resources;

namespace Engine.Actions
{
    class CreateGrassBed : IAction
    {
        private  bool _isInitialized = false;

        private GrassBed _grassBed = null;

        private readonly GrassBedBuilderPlan _grassBedBuilderPlan = new GrassBedBuilderPlan();

        private List<GameObject> _plants = null;

        public string Name
        {
            get { return ActionsResource.BuildGrassBed; }
        }

        public string GetName(IEnumerable<GameObject> objects)
        {
            if (objects.Any(o => o is GrassBed))
            {
                return ActionsResource.ContinueBuilding;
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
                var builderPlan = grassBed.BuilderPlan;
                var objectsLeftToBuild = builderPlan.CurrentStep.GetAvailableObjects(allObjects); //allObjects.OfType<Plant>().Take(grassBed.CountLeftToBuild);
                if (objectsLeftToBuild.Any())
                {
                    yield return objectsLeftToBuild.Union(new[]{grassBed}).ToList();
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

                var objectsLeftToBuild = _grassBedBuilderPlan.CurrentStep.GetAvailableObjects(allObjects);
                if (objectsLeftToBuild.Any())
                {
                    yield return objectsLeftToBuild.ToList();
                }
              /*  var plants = allObjects.OfType<Plant>().Take(GrassBed.CountToBuild);
                if (plants.Any())
                {
                    yield return plants.OfType<GameObject>().ToList();
                }*/
            }
        }

        public double GetTiredness()
        {
            return 0.5;
        }

        protected void Initialize(Hero hero, IEnumerable<GameObject> objects)
        {
            _grassBed = (GrassBed) objects.FirstOrDefault(o => o is GrassBed);

            if (_grassBed == null)
            {
                _plants = _grassBedBuilderPlan.CurrentStep.GetAvailableObjects(objects);
            }
            else
            {
                _plants = _grassBed.BuilderPlan.CurrentStep.GetAvailableObjects(objects);
            }
        }

        private bool DoOnEachAction(Hero hero)
        {
            if (_grassBed == null)
            {
                _grassBed = new GrassBed();
                Game.Map.SetObjectFromDestination(hero.Position, _grassBed);
            }

            _plants = _grassBed.BuilderPlan.CurrentStep.BuildAction(_plants);

            if (_grassBed.BuilderPlan.CurrentStep.IsCompleted)
            {
                _grassBed.BuilderPlan.MoveNextStep();
                return true;
            }
            return _grassBed.IsBuild || !_plants.Any();
        }
    }
}
