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
        public uint Id { get; set; }

        public string Name { get; set; }

        public virtual int Weight { get; set; } = 1;

        public virtual double WeightDbl => 1.0;
        
        public GameObject()
        {
            InitializeBehaviors();
            InitializeProperties();
        }

        public GameObject(IObjectContext context)
        {
            Properties = context.Properties ?? new HashSet<Property>();
            Behaviors = context.Behaviors ?? new HashSet<IBehavior>();

            Id = context.Id;
            Name = context.Name;
            Weight = context.Weight;
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
    }
}
