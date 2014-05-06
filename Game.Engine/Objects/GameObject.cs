using System.Collections.Generic;
using System.Collections.Specialized;

namespace Game.Engine.Objects
{
    public abstract class GameObject
    {
        public HashSet<Property> Properties { get; protected set; }

        public abstract string Name { get; }

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
    }
}
