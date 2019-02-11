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
        public ObjStateProperties GrowingProps { get; set; } = new ObjStateProperties() { TickCount = DayNightCycle.OneSixDayLength, Distribution = DayNightCycle.OneSixDayLength/6, Eternal = false, Id = 0x10001100 };
        public ObjStateProperties StayingProps { get; set; } = new ObjStateProperties() { TickCount = DayNightCycle.HalfDayLength, Distribution = DayNightCycle.HalfDayLength/10, Eternal = false, Id = 0x00001100 };
        public ObjStateProperties DryingProps { get; set; } = new ObjStateProperties() { TickCount = DayNightCycle.OneSixDayLength, Distribution = DayNightCycle.OneSixDayLength/10, Eternal = false, Id = 0x20001100 };
        
        public uint Id { get; set; } = 0x00001100;

        public string Name { get; set; } = Resource.Plant;
        public int Weight { get; set; } = 1;
        public bool NeedKnowledge { get; set; }
        public uint BaseId { get; set; }
        public string BaseName { get; set; } = Resource.Plant;

        public Func<HashSet<Property>> Properties { get; set; } = () => new HashSet<Property>
        {
            Property.Pickable,
            Property.NeedToMakeFireWithWood,
            Property.Regrowable,
            Property.NeedToBuildGrassBed
        };

        public Func<HashSet<IBehavior>> Behaviors { get; set; } = () => new HashSet<IBehavior>
        {
            new BurnableBehavior(300)
        };

        public GameObject Produce()
        {
            return new Plant(this);
        }
    }

    [GenerateMap]
    internal class Plant: FixedObject, IWithObjectWithState
    {
        public ObjectWithState ObjectWithState { get; }

        private readonly PlantContext _plantContext;

        public Plant(PlantContext plantContext) :base(plantContext)
        {
            _plantContext = plantContext;

            ObjectWithState =
                new ObjectWithState(
                    new List<ObjectState>
                    {
                        new ObjectState(ObjectStates.ObjectStates.Growing, plantContext.GrowingProps),
                        new ObjectState(ObjectStates.ObjectStates.Staying, plantContext.StayingProps),
                        new ObjectState(ObjectStates.ObjectStates.Drying, plantContext.DryingProps)
                    }, 
                    false, 
                    OnLastStateFinished);
        }

        public override double WeightDbl
        {
            get
            {
                if (ObjectWithState.CurrentState.Name == ObjectStates.ObjectStates.Growing)
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
            return ObjectWithState.CurrentState?.Id ?? Id;
        }

        public override uint GetBaseCode()
        {
            if (NeedKnowledge)
            {
                return ObjectWithState.CurrentState?.BaseId ?? GetDrawingCode();
            }

            return GetDrawingCode();
        }
    }
}
