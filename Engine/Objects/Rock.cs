using System;
using System.Collections.Generic;
using Engine.Interfaces;
using Engine.Resources;
using Engine.Tools;

namespace Engine.Objects
{
    public class RockContext : IObjectContext
    {
        public GameObject Produce()
        {
            return new Rock(this);
        }

        public Func<HashSet<Property>> Properties { get; set; } = () =>
        new HashSet<Property>
        {
            Property.Pickable,
            Property.NeedToCreateStoneAxe,
            Property.Stone,
            Property.Cracker
        };

        public Func<HashSet<IBehavior>> Behaviors { get; set; } = () => new HashSet<IBehavior>();
        public uint Id { get; set; } = 0x00001000;
        public string Name { get; set; } = Resource.Rock;
        public int Weight { get; set; } = 1;
        public bool NeedKnowledge { get; set; }
        public uint BaseId { get; set; }
    }

    [GenerateMap]
    class Rock : FixedObject
    {
        public Rock(RockContext rockContext) : base(rockContext) 
        {}

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>
            {
                Property.Pickable,
                Property.NeedToCreateStoneAxe,
                Property.Stone,
                Property.Cracker
            };
        }
    }
}
