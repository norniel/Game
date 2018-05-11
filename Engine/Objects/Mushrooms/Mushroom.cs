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
            new EatableBehavior(2),
            new SpoileringBehavior()
        };

        public GameObject Produce()
        {
            return new Mushroom(this);
        }
    }

    [GenerateMap]
    public class Mushroom : FixedObject, IWithObjectWithState
    {
        public ObjectWithState ObjectWithState { get; }

        public override int Weight => 2;

        public uint GrowingId { get; set; }

        public uint BaseId { get; set; }
        public uint BaseGrowingId { get; set; }

        public Mushroom(MushroomContext context) : base(context)
        {
            NeedKnowledge = true;
            KnowledgeKoef = Game.Random.NextDouble();

            GrowingId = context.GrowingId;

            BaseId = 0x00001900;
            BaseGrowingId = 0x10001900;

        ObjectWithState =
                new ObjectWithState(
                    new List<ObjectState>
                    {
                        new ObjectState(ObjectStates.ObjectStates.Growing, context.GrowingProps),
                        new ObjectState(ObjectStates.ObjectStates.Staying, context.StayingProps)
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
            return DrawingCode(GrowingId, Id);
        }

        private uint DrawingCode(uint growingId, uint id)
        {
            if (ObjectWithState.CurrentState.Name == ObjectStates.ObjectStates.Growing)
                return growingId;

            return id;
        }

        public override uint GetBaseCode()
        {
            return DrawingCode(BaseGrowingId, BaseId);
        }

        public override double WeightDbl
        {
            get
            {
                if (ObjectWithState.CurrentState.Name == ObjectStates.ObjectStates.Growing)
                    return 0.5;

                return 1;
            }
        }
    }
}
