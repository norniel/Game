using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Engine.Objects;

namespace Engine.Wrapers
{
    public class RemovableWrapper<T> where T : GameObject
    {
        public T GameObject { get; set; }

        public Action RemoveFromContainer { get; set; }
    }

    public class RemovableObjecctsComparer<T> : IEqualityComparer<RemovableWrapper<T>> where T : GameObject
    {
        public bool Equals(RemovableWrapper<T> x, RemovableWrapper<T> y)
        {
            return x.GameObject.Equals(y.GameObject);
        }

        public int GetHashCode(RemovableWrapper<T> obj)
        {
            return obj.GameObject.GetHashCode();
        }
    }
}
