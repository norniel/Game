using System.Collections.Generic;
using Game.Engine.Objects;

namespace Game.Engine
{
    public class Bag
    {
        public List<GameObject> GameObjects { get; private set; }

        public Bag()
        {
            GameObjects = new List<GameObject>();
        }

        public void Add(IEnumerable<GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Properties.Remove(Property.Pickable);
                gameObject.Properties.Add(Property.Dropable);
            }

            GameObjects.AddRange(gameObjects);
        }
    }
}
