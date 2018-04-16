using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Interfaces;

namespace Engine.Objects
{
    public abstract class GameObject:IRemovableObject
    {
        public HashSet<Property> Properties { get; set; }
        public HashSet<IBehavior> Behaviors { get; set; }
        public uint Id { get; protected set; }

        public abstract string Name { get; }

        public virtual int Weight => 1;

        public virtual double WeightDbl => 1.0;
        
        public GameObject()
        {
            InitializeBehaviors();
            InitializeProperties();
        }

        public virtual void InitializeProperties()
        {
            Properties = new HashSet<Property>();
        }

        public virtual void InitializeBehaviors()
        {
            Behaviors = new HashSet<IBehavior>();
        }

        public virtual uint GetDrawingCode()
        {
            return 0;
        }

        public Action RemoveFromContainer { get; set; }
        public object Quaolity { get; set; }

        public bool HasBehavior(Type type)
        {
            return Behaviors.Any(b => b.GetType() == type);
        }

        public IBehavior GetBehavior(Type type)
        {
            return Behaviors.FirstOrDefault(b => b.GetType() == type);
        }

        public virtual GameObject Clone()
        {
            return null;
        }
    }
}
