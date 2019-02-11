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
        private uint BaseId { get; set; }
        
        public string Name { get; set; }
        private string BaseName { get; set; }

        public virtual int Weight { get; set; } = 1;

        public virtual double WeightDbl => 1.0;

        public bool NeedKnowledge { get; set; } = false;
        public double KnowledgeKoef { get; set; } = 0;
        
        public GameObject()
        {
            InitializeBehaviors();
            InitializeProperties();
        }

        public GameObject(IObjectContext context)
        {
            Properties = context.Properties() ?? new HashSet<Property>();
            Behaviors = context.Behaviors() ?? new HashSet<IBehavior>();

            Id = context.Id;
            BaseId = context.BaseId == 0 ? context.Id : context.BaseId;
            BaseName = string.IsNullOrEmpty(context.BaseName) ? context.Name : context.BaseName; 
            Name = context.Name;
            Weight = context.Weight;

            if(context.NeedKnowledge)
                InitKnowledgeKoef();
        }

        protected void InitKnowledgeKoef()
        {
            NeedKnowledge = true;
            KnowledgeKoef = Game.Random.NextDouble();
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

        public virtual uint GetBaseCode()
        {
            if (NeedKnowledge)
            {
                return BaseId;
            }

            return GetDrawingCode();
        }

        public virtual string GetBaseName()
        {
            if (NeedKnowledge)
            {
                return BaseName;
            }

            return Name;
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
