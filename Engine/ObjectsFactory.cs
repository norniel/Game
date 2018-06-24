using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Engine.Behaviors;
using Engine.Interfaces;
using Engine.Objects;
using Engine.ObjectStates;
using Engine.Resources;

namespace Engine
{
    public class ObjectsFactory
    {
        private Dictionary<string, IObjectContext> _contexts = new Dictionary<string, IObjectContext>()
        {
            { "RaspBerries", new BerryContext(){Id = 0x00000900,Name = "Raspberries", Behaviors = () => new HashSet<IBehavior>{new EatableBehavior(1)}}},
            { Resource.Cone, new BerryContext(){Id = 0x00001700,Name = Resource.Cone, Weight = 2, Properties = () => new HashSet<Property>{Property.Pickable}}},
            { Resource.Apple, new BerryContext(){Name = Resource.Apple, Weight = 2,
                Properties = () => new HashSet<Property>{Property.Pickable,Property.Eatable, Property.Roastable},
                Behaviors  = () => new HashSet<IBehavior>
                    {
                        new RoastBehavior(Resource.RoastedApple),
                        new EatableBehavior(2)
                    }}

            },
            { "Nut", new BerryContext(){Id = 0x00002600,Name = "Nut", Weight = 1,
                Properties = () => new HashSet<Property>{Property.Pickable, Property.Crackable},
                Behaviors  = () => new HashSet<IBehavior>{new CrackableBehavior("Nut Kernel") }
            }},
            { "Nut Kernel", new FixedObjectContext(){Id = 0x00003600,Name = "Nut Kernel", Weight = 1, Properties = () => new HashSet<Property>{Property.Pickable,Property.Eatable},
                Behaviors  = () => new HashSet<IBehavior>{new EatableBehavior(4)}}
            },
            { Resource.RoastedApple, new FixedObjectContext(){Id = 0x00001B00,Name = Resource.RoastedApple, Weight = 2, Properties = () => new HashSet<Property>{Property.Pickable,Property.Eatable},
                Behaviors  = () => new HashSet<IBehavior>{new EatableBehavior(3)}}
            },
            { Resource.RoastedBurovik, new FixedObjectContext(){Id = 0x00001A00,Name = Resource.RoastedBurovik, Weight = 2, Properties = () => new HashSet<Property>{Property.Pickable,Property.Eatable},
                Behaviors  = () => new HashSet<IBehavior>{new EatableBehavior(5)}}
            },
            { "Apple tree", new TreeContext(){Name = "Apple tree", 
                Behaviors  = () => new HashSet<IBehavior>
                {
                    new CollectBehavior<Berry>(Resource.Apple, 2, 4),
                    new CollectBehavior<Branch>("Branch", 1, 4),
                    new CollectBehavior<Twig>("Twig", 2, 16)
                }}
            },
            { "Nut tree", new TreeContext(){Name = "Nut tree", Id =0x00002500, HalfEmptyId = 0x00002500,
                Behaviors  = () => new HashSet<IBehavior>
                {
                    new CollectBehavior<Berry>("Nut", 2, 4),
                    new CollectBehavior<Branch>("Branch", 1, 4),
                    new CollectBehavior<Twig>("Twig", 2, 16)
                }}
            },
            { "Spruce tree", new TreeContext(){Name = "Spruce tree", Id =0x00001600, HalfEmptyId = 0x00001600, EmptyId = 0x00001600,
                    Properties = () => new HashSet<Property>
                    {
                        Property.Cuttable,
                        Property.CollectBerries,
                        Property.CollectBranch,
                        Property.CollectTwig,
                        Property.CollectRoot
                    },
        Behaviors  = () => new HashSet<IBehavior>
                {
                    new CollectBehavior<Berry>("Cone", 2, 4),
                    new CollectBehavior<Branch>("Branch", 1, 4),
                    new CollectBehavior<Twig>("Twig", 2, 16),
                    new CollectBehavior<Root>(Resource.Root, 1, 4)
                }
                }
            },
            { "Muhomor", new MushroomContext(){Name = "Muhomor", Weight = 2, Id = 0x00002700,
                Behaviors  = () => new HashSet<IBehavior>
                    {
                        new RoastBehavior( Resource.RoastedBurovik),
                        new EatableBehavior(2, 50, 1),
                        new SpoileringBehavior()
                    },
                    GrowingProps = new ObjStateProperties() { TickCount = 250, Distribution = 100, Eternal = false, Id = 0x10002700 },
                    StayingProps = new ObjStateProperties() { TickCount = 1000, Distribution = 100, Eternal = false, Id = 0x00002700 }
                }

            },
            { "Poganka", new MushroomContext(){Name = "Poganka", Weight = 2, Id = 0x00002800,
                    Behaviors  = () => new HashSet<IBehavior>
                    {
                        new RoastBehavior( Resource.RoastedBurovik),
                        new EatableBehavior(2, 10, 8),
                        new SpoileringBehavior()
                    },
                    GrowingProps = new ObjStateProperties() { TickCount = 280, Distribution = 100, Eternal = false, Id = 0x10002800 },
                    StayingProps = new ObjStateProperties() { TickCount = 1000, Distribution = 100, Eternal = false, Id = 0x00002800 }
                }
            },
            { "Birch tree", new TreeContext(){Name = "Birch tree", Id =0x00002900, HalfEmptyId = 0x00002900, EmptyId = 0x00002900,
                Behaviors  = () => new HashSet<IBehavior>
                {
                    new CollectBehavior<Branch>("Branch", 1, 4),
                    new CollectBehavior<Twig>("Twig", 2, 16)
                }}
            },
            { "Walnut tree", new TreeContext(){Name = "Walnut tree", Id = 0x00003000, HalfEmptyId = 0x00003000, EmptyId = 0x00003000,
                Behaviors  = () => new HashSet<IBehavior>
                {
                    new CollectBehavior<Berry>("Walnut", 2, 4),
                    new CollectBehavior<Branch>("Branch", 1, 4),
                    new CollectBehavior<Twig>("Twig", 2, 16)
                }}
            },
            { "Hazelnut tree", new TreeContext(){Name = "Hazelnut tree", Id = 0x00003100, HalfEmptyId = 0x00003100, EmptyId = 0x00003100,
                Behaviors  = () => new HashSet<IBehavior>
                {
                    new CollectBehavior<Berry>("Hazelnut", 2, 4),
                    new CollectBehavior<Branch>("Branch", 1, 4),
                    new CollectBehavior<Twig>("Twig", 2, 16)
                }}
            },
            { "Walnut", new BerryContext(){Id = 0x00003400,Name = "Walnut", Weight = 1, NeedKnowledge = true, BaseId = 0x00002600,
                Properties = () => new HashSet<Property>{Property.Pickable, Property.Crackable},
                Behaviors  = () => new HashSet<IBehavior>{new CrackableBehavior("Walnut Kernel") }
            }},
            { "Walnut Kernel", new FixedObjectContext(){Id = 0x00003500,Name = "Walnut Kernel", Weight = 1, NeedKnowledge = true, BaseId = 0x00003600,
                Properties = () => new HashSet<Property>{Property.Pickable,Property.Eatable},
                Behaviors  = () => new HashSet<IBehavior>{new EatableBehavior(4)}}
            },
            { "Hazelnut", new BerryContext(){Id = 0x00003200,Name = "Hazelnut", Weight = 1, NeedKnowledge = true, BaseId = 0x00002600,
                Properties = () => new HashSet<Property>{Property.Pickable, Property.Crackable},
                Behaviors  = () => new HashSet<IBehavior>{new CrackableBehavior("Hazelnut Kernel") }
            }},
            { "Hazelnut Kernel", new FixedObjectContext(){Id = 0x00003300,Name = "Hazelnut Kernel", Weight = 1, NeedKnowledge = true, BaseId = 0x00003600,
                Properties = () => new HashSet<Property>{Property.Pickable,Property.Eatable},
                Behaviors  = () => new HashSet<IBehavior>{new EatableBehavior(4)}}
            },
            { "Fern", new PlantContext()
                {
                    Id = 0x00003700,Name = "Fern", Weight = 1, NeedKnowledge = true, /*BaseId = 0x00001100,*/
                    GrowingProps  = new ObjStateProperties() { TickCount = 300, Distribution = 50, Eternal = false, Id =0x10003700, BaseId = 0x10001100 },
                    StayingProps = new ObjStateProperties() { TickCount = 1000, Distribution = 100, Eternal = false, Id = 0x00003700, BaseId = 0x00001100 },
                    DryingProps = new ObjStateProperties() { TickCount = 300, Distribution = 30, Eternal = false, Id = 0x20003700, BaseId = 0x20001100 }

                }

            }
        };

        public readonly List<string> ObjectsToGenMap = new List<string>()
        {
            "Apple tree",
            "Spruce tree",
            Resource.Burovik,
            Resource.Bush,
           // Resource.Plant,
            Resource.Rock,
            "Muhomor",
            "Poganka",
            "Birch tree",
            "Hazelnut tree",
            "Walnut tree",
            "Fern"
        };

        public ObjectsFactory()
        {
            Assembly.GetExecutingAssembly().GetTypes().Where(
                    type => typeof(IObjectContext).IsAssignableFrom(type) && type != typeof(FixedObjectContext) && !type.IsInterface)
                .Select(type => Activator.CreateInstance(type) as IObjectContext)
                .ToList()
                .ForEach(context =>
                {
                    if (!_contexts.ContainsKey(context.Name))
                    {
                        _contexts.Add(context.Name, context);
                    }
                });
        }

        public GameObject Produce(string name)
        {
            IObjectContext context;
            _contexts.TryGetValue(name, out context);

            return context?.Produce();
        }
    }
}
