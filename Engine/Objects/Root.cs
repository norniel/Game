using System;
using System.Collections.Generic;
using Engine.Behaviors;
using Engine.Interfaces;
using Engine.Resources;

namespace Engine.Objects
{
    public class RootContext : IObjectContext
    {
        public uint Id { get; set; } = 0x00002400;

        public string Name { get; set; } = Resource.Root;
        public int Weight { get; set; } = 1;

        public Func<HashSet<Property>> Properties { get; set; } = () => new HashSet<Property>
        {
            Property.Diggable
        };

        public Func<HashSet<IBehavior>> Behaviors { get; set; } = () => new HashSet<IBehavior>{};

        public GameObject Produce()
        {
            return new Root(this);
        }
    }

    class Root : FixedObject
    {
        private RootContext _rootContext;

        public Root(RootContext rootContext) : base(rootContext)
        {
            _rootContext = rootContext;
        }
    }
}
