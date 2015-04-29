using System.Collections.Generic;
using Game.Engine.Heros;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;

namespace Game.Engine.Actions
{
    abstract class LongActionBase: IAction
    {
        public abstract string Name { get; }

        public abstract string GetName(IEnumerable<GameObject> objects);

        public abstract bool IsApplicable(Property property);

        public virtual bool Do(Hero hero, IEnumerable<GameObject> objects)
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

            return isOver;
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

        public abstract IEnumerable<List<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero);
        public abstract double GetTiredness();

        protected abstract int ElapsedActionTime { get; set; }

        protected abstract int TotalActionTime { get; }

        protected bool _isInitialized = false;
    }
}
