﻿using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Resources;

namespace Engine
{
    internal class PickAction : IAction
    {
        public string Name => ActionsResource.Pick;

        public bool Do(Hero hero, IList<GameObject> objects)
        {
            hero.AddToBag(objects);

            return true;
        }

        public Knowledges GetKnowledge()
        {
            return Knowledges.Nothing;
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.All(x => x.Properties.Contains(Property.Pickable));// && hero.HasEmpptyContainer;
        }

        public IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            yield return objects.Where(x => x.Properties.Contains(Property.Pickable)).ToList();// && hero.HasEmpptyContainer;
        }

        public double GetTiredness()
        {
            return 0.5;
        }

        public string GetName(IEnumerable<GameObject> objects)
        {
            return Name;
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.Pickable;
        }

        public Point GetDestination(Point destination, FixedObject destObject, Hero hero)
        {
            return destination;
        }
    }
}
