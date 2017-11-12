using System;
using System.Collections.Generic;
using Engine.Interfaces;

namespace Engine.Objects
{
    public abstract class GameObject:IRemovableObject
    {
        public HashSet<Property> Properties { get; protected set; }
        public uint Id { get; protected set; }

        public abstract string Name { get; }

        public virtual int Weight => 1;

        public GameObject()
        {
            InitializeProperties();
        }

        public virtual void InitializeProperties()
        {
            Properties = new HashSet<Property>();
        }

        public virtual uint GetDrawingCode()
        {
            return 0;
        }

        public Action RemoveFromContainer { get; set; }
        public object Quaolity { get; set; }
    }
}
