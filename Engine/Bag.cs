using System.Collections.Generic;
using Engine.Objects;

namespace Engine
{
    public class Bag: Container
    {
        public List<GameObject> GameObjects { get; private set; }

        public Bag(int stackQuantity, int stackCapacity):base(stackQuantity, stackCapacity)
        {
            GameObjects = new List<GameObject>();
        }

        public void Add(IEnumerable<GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                this.Add(gameObject);
            }
        }

        public override bool Add(GameObject gameObject)
        {
            if (!base.Add(gameObject))
                return false;

            if (gameObject.Properties.Contains(Property.Pickable))
            {
                gameObject.Properties.Remove(Property.Pickable);
                if (!gameObject.Properties.Contains(Property.Dropable))
                    gameObject.Properties.Add(Property.Dropable);
            }

            if (gameObject.RemoveFromContainer != null)
            {
                gameObject.RemoveFromContainer();
            }

            gameObject.RemoveFromContainer = (() =>
            {
                this.Remove(gameObject);
            });

            GameObjects.Add(gameObject);

            return true;
        }

        public override void Remove(GameObject gameObject)
        {
            base.Remove(gameObject);
            this.GameObjects.Remove(gameObject);
        }
    }
}
