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
            bool isOver = false;

            if (TotalActionTime <= ElapsedActionTime)
            {
                DoLast(hero, objects);
                ElapsedActionTime = 0;
                return true;
            }

            isOver = DoNotLast(hero, objects);
            ElapsedActionTime++;

            return isOver;
        }

        protected abstract bool DoNotLast(Hero hero, IEnumerable<GameObject> objects);

        protected abstract void DoLast(Hero hero, IEnumerable<GameObject> objects);

        public abstract bool CanDo(Hero hero, IEnumerable<GameObject> objects);

        public abstract IEnumerable<List<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero);

        protected abstract int ElapsedActionTime { get; set; }

        protected abstract int TotalActionTime { get; }
    }
}
