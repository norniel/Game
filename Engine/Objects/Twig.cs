using System.Collections.Generic;
using Engine.Behaviors;
using Engine.Resources;

namespace Engine.Objects
{
    class Twig : FixedObject
    {
        public Twig() 
        {
            IsPassable = true;

            Size = new Size(1, 1);

            Id = 0x00001C00;
        }

        public override int Weight => 1;

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>
            {
               Property.Pickable
            };
        }

        public override void InitializeBehaviors()
        {
            base.InitializeBehaviors();
            Behaviors.Add(new BurnableBehavior(100));
        }
        
        public override string Name => Resource.Twig;

        public override GameObject Clone()
        {
            return new Twig();
        }
    }
}
