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
            GameObjects.AddRange(gameObjects);
        }
    }
}
