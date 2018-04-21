using System;
using System.Collections.Generic;
using Engine.Behaviors;
using Engine.Interfaces;
using Engine.Resources;

namespace Engine.Objects
{
    public class TwigContext : IObjectContext
    {
        public uint Id { get; set; } = 0x00001C00;

        public string Name { get; set; } = Resource.Twig;
        public int Weight { get; set; } = 1;

        public Func<HashSet<Property>> Properties { get; set; } = () => new HashSet<Property>
        {
            Property.Pickable
        };

        public Func<HashSet<IBehavior>> Behaviors { get; set; } = () => new HashSet<IBehavior>
        {
            new BurnableBehavior(100)
        };

        public GameObject Produce()
        {
            return new Twig(this);
        }
    }

    class Twig : FixedObject
    {
        private readonly TwigContext _twigContext;

        public Twig(TwigContext twigContext): base(twigContext)
        {
            _twigContext = twigContext;
        }
    }
}
