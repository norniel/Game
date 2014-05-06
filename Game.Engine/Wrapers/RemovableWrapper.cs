using System;
using Game.Engine.Objects;

namespace Game.Engine.Wrapers
{
    public class RemovableWrapper<T> where T : GameObject
    {
        public T GameObject { get; set; }

        public Action RemoveFromContainer { get; set; }
    }
}
