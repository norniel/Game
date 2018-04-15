using System.Collections.Generic;
using Engine.Behaviors;
using Engine.Resources;
using Engine.Tools;

namespace Engine.Objects
{
    [GenerateMap]
    class Bush : FixedObject
    {
        private int _berriesCount = 2;
        private int _branchesCount = 4;

        public Bush()
        {
            IsPassable = false;

            Size = new Size(1, 1);

            Id = 0x00001200;
        }

        public override string Name => Resource.Bush;

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>
            {
                Property.Cuttable,
                Property.CollectBerries,
                Property.CollectBranch
            };
        }

        public override void InitializeBehaviors()
        {
            base.InitializeBehaviors();
            Behaviors.Add(new CollectBehavior<Berry>(new RaspBerries(), 2, _berriesCount));
            Behaviors.Add(new CollectBehavior<Branch>(new Branch(), 2, _branchesCount));
        }
    }
}
