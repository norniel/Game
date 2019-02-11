using System.Collections.Generic;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Tools;

namespace Engine.Actions
{
    abstract class LongActionBase: IAction
    {
        public abstract string Name { get; }

        public abstract string GetName(IEnumerable<GameObject> objects, Hero hero);

        public abstract bool IsApplicable(Property property);

        public Knowledges GetKnowledge()
        {
            return Knowledges.Nothing;
        }

        public virtual IActionResult Do(Hero hero, IList<GameObject> objects)
        {
            if (!_isInitialized)
            {
                Initialize(hero, objects);
                _isInitialized = true;
            }

            bool isOver = false;

            if (TotalActionTime <= ElapsedActionTime)
            {
                DoLast(hero, objects);
                isOver = true;
            }
            else
            {
                isOver = DoNotLast(hero, objects);
                ElapsedActionTime++;
            }

            if (isOver)
            {
                ClearAfterLast();
            }

            return isOver ? (IActionResult)new FinishedActionResult() : new UnFinishedActionResult();
        }

        protected virtual void Initialize(Hero hero, IEnumerable<GameObject> objects)
        {}

        protected virtual void ClearAfterLast()
        {
            ElapsedActionTime = 0;
            _isInitialized = false;
        }

        protected abstract bool DoNotLast(Hero hero, IEnumerable<GameObject> objects);

        protected abstract void DoLast(Hero hero, IEnumerable<GameObject> objects);

        public abstract bool CanDo(Hero hero, IEnumerable<GameObject> objects);

        public abstract IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects,
            Hero hero);
        public abstract double GetTiredness();

        protected abstract int ElapsedActionTime { get; set; }

        protected abstract int TotalActionTime { get; }

        protected bool _isInitialized;

        public Point GetDestination(Point destination, FixedObject destObject, Hero hero)
        {
            return destination;
        }
    }
}
