using System;
using System.Collections.Generic;
using Engine.Behaviors;
using Engine.Interfaces;
using Engine.ObjectStates;
using Engine.Resources;
using Engine.Tools;

namespace Engine.Objects
{
    public class PlantContext : IObjectContext
    {
        public ObjStateProperties GrowingProps { get; set; } = new ObjStateProperties() { TickCount = 300, Distribution = 50, Eternal = false };
        public ObjStateProperties StayingProps { get; set; } = new ObjStateProperties() { TickCount = 1000, Distribution = 100, Eternal = false };
        public ObjStateProperties DryingProps { get; set; } = new ObjStateProperties() { TickCount = 300, Distribution = 30, Eternal = false };

        public uint GrowingId { get; set; } = 0x10001100;
        public uint DryingId { get; set; } = 0x20001100;

        public uint Id { get; set; } = 0x00001100;

        public string Name { get; set; } = Resource.Plant;
        public int Weight { get; set; } = 1;

        public HashSet<Property> Properties { get; set; } = new HashSet<Property>
        {
            Property.Pickable,
            Property.NeedToMakeFireWithWood,
            Property.Regrowable,
            Property.NeedToBuildGrassBed
        };

        public HashSet<IBehavior> Behaviors { get; set; } = new HashSet<IBehavior>
        {
            new BurnableBehavior(300)
        };

        public GameObject Produce()
        {
            return new Plant(this);
        }
    }

    [GenerateMap]
    internal class Plant: FixedObject
    {
        private ObjectWithState ObjectWithState { get; }

        private readonly PlantContext _plantContext;

        public Plant(PlantContext plantContext) :base(plantContext)
        {
            _plantContext = plantContext;

            ObjectWithState =
                new ObjectWithState(
                    new List<IObjectState>
                    {
                        new Growing {TickCount = plantContext.GrowingProps.TickCount, Distribution = plantContext.GrowingProps.Distribution, Eternal = plantContext.GrowingProps.Eternal},
                        new Staying {TickCount = plantContext.StayingProps.TickCount, Distribution = plantContext.StayingProps.Distribution, Eternal = plantContext.StayingProps.Eternal},
                        new Drying {TickCount = plantContext.DryingProps.TickCount, Distribution = plantContext.DryingProps.Distribution, Eternal = plantContext.DryingProps.Eternal}
                    }, 
                    false, 
                    OnLastStateFinished);
        }

        public Plant(): this(new PlantContext())
        {}

        public override double WeightDbl
        {
            get
            {
                if (ObjectWithState.CurrentState is Growing)
                {
                    var tToNext = ObjectWithState.TicksToNextState;
                    var tCount = ObjectWithState.CurrentState.TickCount;

                    return tCount > 0 ? (tToNext/tCount) : 0;
                }

                return 1.0;
            }
        }

        public void OnLastStateFinished()
        {
            RemoveFromContainer?.Invoke();
        }

        public override uint GetDrawingCode()
        {
            if (ObjectWithState.CurrentState is Growing)
                return _plantContext.GrowingId;

            if (ObjectWithState.CurrentState is Drying)
                return _plantContext.DryingId;

            return Id;
        }
    }
}
