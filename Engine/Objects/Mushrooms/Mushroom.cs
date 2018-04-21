using System;
using System.Collections.Generic;
using Engine.Behaviors;
using Engine.Interfaces;
using Engine.ObjectStates;
using Engine.Resources;
using Engine.Tools;

namespace Engine.Objects
{
    public class MushroomContext : IObjectContext
    {
        public ObjStateProperties GrowingProps { get; set; } = new ObjStateProperties() {TickCount = 300, Distribution = 50, Eternal = false};
        public ObjStateProperties StayingProps { get; set; } = new ObjStateProperties() {TickCount = 1000, Distribution = 100, Eternal = false };

        public uint GrowingId { get; set; } = 0x10001900;
        
        public uint Id { get; set; } = 0x00001900;

        public string Name { get; set; } = Resource.Burovik;
        public int Weight { get; set; } = 1;

        public Func<HashSet<Property>> Properties { get; set; } = () => new HashSet<Property>
        {
            Property.Pickable,
            Property.Regrowable,
            Property.Eatable,
            Property.Roastable
        };

        public Func<HashSet<IBehavior>> Behaviors { get; set; } = () => new HashSet<IBehavior>
        {
            new RoastBehavior( Resource.RoastedBurovik),
            new EatableBehavior(2)
        };

        public GameObject Produce()
        {
            return new Mushroom(this);
        }
    }

    [GenerateMap]
    public class Mushroom : FixedObject
    {
        private ObjectWithState ObjectWithState { get; }

        public override int Weight => 2;

        public uint GrowingId { get; set; }

        private MushroomContext _mushroomContext;

        public Mushroom(MushroomContext context) : base(context)
        {
            _mushroomContext = context;

            GrowingId = context.GrowingId;

            ObjectWithState =
                new ObjectWithState(
                    new List<IObjectState>
                    {
                        new Growing {TickCount = context.GrowingProps.TickCount, Distribution = context.GrowingProps.Distribution, Eternal = context.GrowingProps.Eternal},
                        new Staying {TickCount = context.StayingProps.TickCount, Distribution = context.StayingProps.Distribution, Eternal = context.StayingProps.Eternal}
                    },
                    false,
                    OnLastStateFinished);
        }
        
        public void OnLastStateFinished()
        {
            RemoveFromContainer?.Invoke();
        }
        
        public override uint GetDrawingCode()
        {
            if (ObjectWithState.CurrentState is Growing)
                return GrowingId;

            return Id;
        }
        
        public override double WeightDbl
        {
            get
            {
                if (ObjectWithState.CurrentState is Growing)
                    return 0.5;

                return 1;
            }
        }
    }
}
