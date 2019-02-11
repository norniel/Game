using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Objects.LargeObjects;
using Engine.Objects.LargeObjects.Builder;
using Engine.Resources;
using Engine.Tools;

namespace Engine.Actions
{
    abstract class CreateObjectWithPlan<T, TPlan>:IAction 
        where T:LargeObjectInner, new() 
        where TPlan:BuilderPlan, new()
    {
        private bool _isInitialized;

        private T _objectWithPlan;

        private readonly BuilderPlan _builderPlan = new TPlan();

        private List<GameObject> _objects;

        public string Name => _builderPlan.Name;

        public string GetName(IEnumerable<GameObject> objects, Hero hero)
        {
            if (objects.Any(o => o is T))
            {
                return ActionsResource.ContinueBuilding;
            }

            return Name;
        }

        public Knowledges GetKnowledge()
        {
            return Knowledges.Nothing;
        }

        public abstract bool IsApplicable(Property property);

        public IActionResult Do(Hero hero, IList<GameObject> objects)
        {
            if (!_isInitialized)
            {
                Initialize(hero, objects);
                _isInitialized = true;
            }

            bool isOver = DoOnEachAction(hero);

            if (isOver)
            {
                _objects = null;
                _objectWithPlan = null;
                _isInitialized = false;
            }

            return isOver ? (IActionResult)new FinishedActionResult() : new UnFinishedActionResult();
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            var allObjects = objects.Union(hero.GetContainerItems()).Distinct().ToArray();
            var objectWithPlan = (T)allObjects.FirstOrDefault(gb => gb is T);

            if (objectWithPlan != null && !objectWithPlan.IsBuild)
            {
                var builderPlan = objectWithPlan.BuilderPlan;
                var objectsLeftToBuild = builderPlan.CurrentStep.GetAvailableObjects(allObjects); 
                if (objectsLeftToBuild.Any())
                {
                    yield return objectsLeftToBuild.Union(new[] { objectWithPlan }).ToList();
                }
            }
            else if (objectWithPlan == null)
            {
                var cell = Map.PointToCell(hero.Position);
               // var mapSize = Game.Map.GetSize();

                if (!_builderPlan.CheckAvailablePlace(cell))
                    yield break;
                /*
                var objectOnPlace = Game.Map.GetObjectFromCell(cell);
                var objectOnNextPlace = Game.Map.GetObjectFromCell(new Point(cell.X, cell.Y + 1));

                if (objectOnPlace != null || objectOnNextPlace != null)
                {
                    yield break;
                }
                */
                var objectsLeftToBuild = _builderPlan.CurrentStep.GetAvailableObjects(allObjects);
                if (objectsLeftToBuild.Any())
                {
                    yield return objectsLeftToBuild.ToList();
                }
            }
        }

        public double GetTiredness()
        {
            return 0.5;
        }

        protected void Initialize(Hero hero, IList<GameObject> objects)
        {
            _objectWithPlan = (T)objects.FirstOrDefault(o => o is T);

            _objects = _objectWithPlan == null
                ? _builderPlan.CurrentStep.GetAvailableObjects(objects)
                : _objectWithPlan.BuilderPlan.CurrentStep.GetAvailableObjects(objects);
        }

        private bool DoOnEachAction(Hero hero)
        {
            if (_objectWithPlan == null)
            {
                _objectWithPlan = new T();
                Game.Map.SetHObjectFromDestination(hero.Position, _objectWithPlan);
            }

            _objects = _objectWithPlan.BuilderPlan.CurrentStep.BuildAction(_objects, hero.HeroLifeCycle.Tiredness);

            if (_objectWithPlan.BuilderPlan.CurrentStep.IsCompleted)
            {
                _objectWithPlan.BuilderPlan.MoveNextStep();
                return true;
            }
            return _objectWithPlan.IsBuild || !_objects.Any();
        }

        public Point GetDestination(Point destination, FixedObject destObject, Hero hero)
        {
            return destination;
        }
    }
}
