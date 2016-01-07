using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Engine.Interfaces;

namespace Engine.Objects
{
    public abstract class GameObject:IRemovableObject
    {
        public HashSet<Property> Properties { get; protected set; }

        public abstract string Name { get; }

        public virtual int Weight { get { return 1; } }

        public GameObject()
        {
            this.InitializeProperties();
        }

        public virtual void InitializeProperties()
        {
            this.Properties = new HashSet<Property>();
        }

        public virtual uint GetDrawingCode()
        {
            return 0;
        }

        public Action RemoveFromContainer { get; set; }
    }
}
