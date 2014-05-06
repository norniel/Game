using System;
using Game.Engine.Objects;

namespace Game.Engine.Wrapers
{
    public class RemovableWrapper<T> where T : GameObject
    {
        public GameObject GameObject { get; set; }

        public Action<T> RemoveFromContainer { get; set; }
    }
}
