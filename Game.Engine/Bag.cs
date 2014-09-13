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
                if (gameObject.Properties.Contains(Property.Pickable))
                {
                    gameObject.Properties.Remove(Property.Pickable);
                    if (!gameObject.Properties.Contains(Property.Dropable))
                        gameObject.Properties.Add(Property.Dropable);
                }
            }

            GameObjects.AddRange(gameObjects);
        }

        public void Add(GameObject gameObject)
        {
            if (gameObject.Properties.Contains(Property.Pickable))
            {
                gameObject.Properties.Remove(Property.Pickable);
                if (!gameObject.Properties.Contains(Property.Dropable))
                    gameObject.Properties.Add(Property.Dropable);
            }

            GameObjects.Add(gameObject);
        }
    }
}
