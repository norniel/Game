using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using Engine.BridgeObjects;
using Engine.Heros;
using Engine.Interfaces;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Objects.LargeObjects;
using Engine.Tools;
using Unity;
using Unity.Lifetime;

namespace Engine
{
    //todo
    //1 Variety of species
    //10 Containers
    //11 Built objects    
    //6 Soil
    //7 Water
    //2 Lifecycle of species
    //9 Alive species
    //3 Seasons
    //4 Temperature, weather
    //5 Surface variety
    //8 Physics in states of objects
    //12 Hero properties
    //13 Day and Night
    //14 Hostile creatures
    
    //Container- quantity of stacks and capacity of stack. MaxContainerCapacity = quantityOfStacks * capacityOfStack
    //sum of items weight = containerWeight
    //Container is full = containerWeight >= MaxContainerCapacity

    //small items should appear only when smbody is looking at the.
    //they should be kept for a period of time and then disapper
    //there should be 2 arrays in the cell - with constatly kept items and temporarily kept items

    public class Game
    {
        private Rect _curRect;

        internal static Map Map;

        internal static ObjectsFactory Factory;

        public const int TimeStep = 100;

        private readonly Hero _hero;

        private readonly IDrawer _drawer;

        private IActionRepository ActionRepository { get; }

        private bool _isPaused;

        public const bool IsShowBase = true;

       // private readonly StateQueueManager _stateQueueManager;

       //todo - change to lazy
        internal static StateQueueManager StateQueueManager;
        internal static PlannedQueueManager PlannedQueueManager;

        //todo - change to lazy
        internal static IObservable<long> Intervals;

        [ThreadStatic] private static Random _random;

        internal static Random Random => _random ?? (_random = new Random());

        private readonly DayNightCycle _dayNightCycle;
        private bool _isKnowledgesShown;

        public Game(IDrawer drawer, uint width, uint height)
        {
            _curRect.Width = width;
            _curRect.Height = height;

            Intervals = Observable.Interval(TimeSpan.FromMilliseconds(TimeStep))
                .Where(t => !_isPaused);

            var unityContainer = new UnityContainer();
            RegisterInUnityContainer(unityContainer);

            StateQueueManager = unityContainer.Resolve<StateQueueManager>();
            PlannedQueueManager = unityContainer.Resolve<PlannedQueueManager>();

            Map = unityContainer.Resolve<Map>();
            Factory = unityContainer.Resolve<ObjectsFactory>();

            var loadSaveManager = new LoadSaveManager();
            loadSaveManager.LoadSnapshot(Map, unityContainer);

            _hero = unityContainer.Resolve<Hero>();
            _hero.Map = Map;

            ActionRepository = unityContainer.Resolve<IActionRepository>();

            _drawer = drawer;

            Intervals.Subscribe(StateQueueManager);
            Intervals.Subscribe(PlannedQueueManager);

            _dayNightCycle = unityContainer.Resolve<DayNightCycle>();
            Intervals.Subscribe(_dayNightCycle);

            var heroLifeCycle = unityContainer.Resolve<HeroLifeCycle>();
            Intervals.Subscribe(heroLifeCycle);
        }

        private void RegisterInUnityContainer(IUnityContainer unityContainer)
        {

            unityContainer.RegisterInstance(typeof(Map), new Map(_curRect));
            unityContainer.RegisterInstance(typeof (StateQueueManager), new StateQueueManager());
            unityContainer.RegisterInstance(typeof(PlannedQueueManager), new PlannedQueueManager());
            unityContainer.RegisterType(typeof(IActionRepository), typeof(ActionRepository), new ContainerControlledLifetimeManager());
            unityContainer.RegisterType(typeof(HeroLifeCycle), typeof(HeroLifeCycle), new ContainerControlledLifetimeManager());
            unityContainer.RegisterType(typeof(DayNightCycle), typeof(DayNightCycle), new ContainerControlledLifetimeManager());

            var hero = new Hero();
            unityContainer.BuildUp(hero);
            unityContainer.RegisterInstance(typeof(Hero), hero);

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && !type.IsInterface && typeof(IAction).IsAssignableFrom(type)))
            {
                unityContainer
                    .RegisterType(typeof(IAction), type, type.ToString());
            }
        }


        public void LClick(Point visibleDestination)
        {
            if (_hero.IsUnconscios() || _isPaused)
                return;

            var destination = Map.GetRealDestinationFromVisibleDestination(visibleDestination);

            if (!Map.PointInVisibleRect(destination))
                return;

            MoveToDest(destination);
        }

        public void RClick(Point destination)
        {
            if (_hero.IsUnconscios() || _isPaused)
                return;

            if (destination.X < 0 || destination.X >= Map.VisibleRect.Width || destination.Y < 0 || destination.Y >= Map.VisibleRect.Height)
                return;

            ShowActions(destination);
        }

        private void ShowActions(Point destination)
        {
            _drawer.DrawMenu(destination.X, destination.Y, GetActions(destination));
        }

        private IEnumerable<ClientAction> GetActions(Point visibleDestination)
        {
            var destination = Map.GetRealDestinationFromVisibleDestination(visibleDestination);
            var destObject = Map.GetHRealObjectFromDestination(destination);

            if (destObject == null)
            {
                return new List<ClientAction>
                {
                    new ClientAction
                    {
                        Name = "Go",
                        //CanDo = true,
                        Do = () => MoveToDest(destination)
                    }
                };
            }

            var possibleActions = ActionRepository.GetPossibleActions(_hero, destObject).ToList();

            var objects = new List<GameObject>(new[] {destObject});

          //  var removableObjects = objects.Select(o => this.PrepareRemovableObject(o, destination));
            return possibleActions.SelectMany(pa =>
            {
                var dest = pa.GetDestination(destination, destObject, _hero);
                return pa.GetActionsWithNecessaryObjects(objects, _hero).Select(objectsForAction =>
                    new ClientAction
                    {
                        Name = pa.GetName(objectsForAction, _hero),
                        //CanDo = pa.CanDo(_hero, objects),
                        Do = () => MoveAndDoAction(pa, dest, objectsForAction)
                    }
                );

            });
        }

        public void MoveToDest(Point destination)
        {
            _hero.StartMove(destination, Map.GetEasiestWay(_hero.Position, destination));
        }

        private void MoveAndDoAction(IAction action, Point destination,
            IList<GameObject> objects)
        {
            _hero.StartMove(destination, Map.GetEasiestWay(_hero.Position, destination));
            _hero.Then().StartActing(action, destination, objects);
        }

        private void DoAction(IAction action, IList<GameObject> objects)
        {
            _hero.StartActing(action, null, objects);
        }

        public void DrawChanges()
        {
            Map.RecalcVisibleRect(_hero.Position);

            _drawer.Clear();
            if (_isKnowledgesShown)
            {
                _drawer.DrawKnowledges();
            }
            else if (_hero.IsHalt())
            {
                SetPaused(true);
                _drawer.DrawHaltScreen(_hero.GetAllKnowledges(), newKnowledges =>
                {
                    _hero.RewriteKnowledges(newKnowledges);
                    SetPaused(false);
                });
            }
            else
            {
                DrawSurface();
            }

            IEnumerable<MenuItems> groupedItems = _hero.GetContainerItems()
                .GroupBy(go => GetDrawingCode(go),
                    (id, gos) =>
                    new MenuItems
                    {
                        Name = $"{GetScreenName(gos.First())}({gos.Count()})",
                        Id = id,
                        GetClientActions = GetFuncForClientActions(gos.First())
                    });

            _drawer.DrawContainer(groupedItems);

            _drawer.DrawHeroProperties(_hero.GetProperties());

            _drawer.DrawTime(_dayNightCycle.CurrentGameDate);
        }

        private void DrawSurface()
        {
            _drawer.DrawSurface(_curRect.Width, _curRect.Height);

            var visibleCells = Map.RectToCellRect(Map.VisibleRect);

            var lightObjectsList = new List<BurningProps>();

            //var mapSize = _map.GetSize();
            for (int j = visibleCells.Top; j < visibleCells.Top + visibleCells.Height; j++)
            {
                for (int i = visibleCells.Left; i < visibleCells.Left + visibleCells.Width; i++)
                {
                    var point = new Point(i, j);
                    var gameObject = Map.GetHObjectFromCell(point);

                    if (gameObject == null) continue;
                    if (gameObject is LargeObjectOuterAbstract largeObjectOuter && !largeObjectOuter.isLeftCorner)
                        continue;

                    var visibleDestination = Map.GetVisibleDestinationFromRealDestination(Map.CellToPoint(point));

                    var drawingCode = GetDrawingCode(gameObject);

                    _drawer.DrawObject(drawingCode, visibleDestination.X, visibleDestination.Y, gameObject.Height);

                    if (gameObject is IBurning burning)
                    {
                        lightObjectsList.Add(new BurningProps(visibleDestination, burning.LightRadius));
                    }
                }

                if (j * Map.CellMeasure <= _hero.Position.Y && (j + 1) * Map.CellMeasure > _hero.Position.Y)
                {
                    _drawer.DrawHero(
                        Map.GetVisibleDestinationFromRealDestination(_hero.Position), 
                        _hero.Angle, 
                        _hero.PointList.Select(p => Map.GetVisibleDestinationFromRealDestination(p)).ToList(),
                        _hero.IsHorizontal());
                }
            }

            var mobileObjects = Map.GetMobileObjects();
            foreach (var mobileObject in mobileObjects)
            {
                if (Map.PointInVisibleRect(mobileObject.Position))
                {
                    var visibleDestination = Map.GetVisibleDestinationFromRealDestination(mobileObject.Position);
                    _drawer.DrawObject(mobileObject.GetDrawingCode(), visibleDestination.X, visibleDestination.Y, mobileObject.Height);
                }
            }

            //  _drawer.DrawHero(Map.GetVisibleDestinationFromRealDestination(_hero.Position), _hero.Angle, _hero.PointList.Select(p => Map.GetVisibleDestinationFromRealDestination(p)).ToList(), _hero.IsHorizontal());

            if (IsHeroInInnerMap())
            {
                var innerMapSize = Map.GetInnerMapRect();
                var visibleDestination = Map.GetVisibleDestinationFromRealDestination(Map.CellToPoint(new Point(innerMapSize.Left, innerMapSize.Top)));
                _drawer.DrawShadow(visibleDestination, new Size(innerMapSize.Width, innerMapSize.Height));
            }
            else
            {
                _drawer.DrawDayNight(_dayNightCycle.Lightness(), lightObjectsList);
            }

            _drawer.DrawActing(_hero.State.ShowActing);
        }

        private uint GetDrawingCode(GameObject gameObject)
        {
            return _hero.IsBaseToShow(gameObject) ? gameObject.GetBaseCode() : gameObject.GetDrawingCode();
        }

        private string GetScreenName(GameObject gameObject)
        {
            return _hero.IsBaseToShow(gameObject) ? gameObject.GetBaseName() : gameObject.Name;
        }

        private Func<IEnumerable<ClientAction>> GetFuncForClientActions(GameObject first)
        {
            return () =>
            {
                if (_hero.IsUnconscios())
                    return new List<ClientAction>();

                var possibleActions = ActionRepository.GetPossibleActions(_hero, first).ToList();

                var objects = new List<GameObject>(new[] {first});

                //  var removableObjects = objects.Select(o => this.PrepareRemovableObject(o));
                return possibleActions.SelectMany(pa =>
                {
                    return pa.GetActionsWithNecessaryObjects(objects, _hero).Select(objectsForAction =>
                        new ClientAction
                        {
                            Name = pa.GetName(objectsForAction, _hero),
                            //CanDo = pa.CanDo(_hero, objects),
                            Do = () => DoAction(pa, objectsForAction)
                        }
                    );

                });
            };
        }

        private bool IsHeroInInnerMap()
        {
            var heroCell = _hero.PositionCell;
            return Map.CellInInnerMap(heroCell);
        }

        public static void AddToGame(Hero hero, FixedObject gameObject)
        {
            if (!hero.AddToBag(gameObject))
            {
                Map.SetHObjectFromDestination(hero.Position, gameObject);
            }
        }

        public void SetPaused()
        {
            if (_hero.IsHalt())
                return;

            SetPaused(!_isPaused);
        }

        private void SetPaused(bool isPaused)
        {
            if (isPaused == _isPaused)
                return;

            _isPaused = isPaused;
            _drawer.SetPaused(_isPaused);

            if (!isPaused)
                ShowKnowledges(false);
        }

        public void ShowKnowledges()
        {
            if (_hero.IsHalt())
                return;

            var isKnowledgesShown = !_isKnowledgesShown;
            ShowKnowledges(isKnowledgesShown);
            SetPaused(_isKnowledgesShown);
        }

        private void ShowKnowledges(bool isKnowledgesShown)
        {
            _isKnowledgesShown = isKnowledgesShown;
            _drawer.ShowKnowledges(_isKnowledgesShown, _hero.GetAllKnowledges());
        }
    }

    public class MenuItems
    {
        public string Name { get; set; }
        public uint Id { get; set; }
        public Func<IEnumerable<ClientAction>> GetClientActions { get; set; }
    }
}
