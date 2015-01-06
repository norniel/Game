using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using Game.Engine.Actions;
using Game.Engine.BridgeObjects;
using Game.Engine.Heros;
using Game.Engine.Interfaces;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Wrapers;
using Game.Engine.Objects;
using Microsoft.Practices.Unity;

namespace Game.Engine
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
    public class Game
    {
        private Rect curRect;

        internal static Map Map;

        public const int TimeStep = 100;

        private Dictionary<uint, Cell> _cellSamples;

        private LoadSaveManager loadSaveManager;

        private readonly Hero _hero;

        private readonly IDrawer _drawer;

        private IActionRepository ActionRepository { get; set; }

       // private readonly StateQueueManager _stateQueueManager;

        private UnityContainer _unityContainer;

        //todo - change to lazy
        internal static StateQueueManager StateQueueManager = null;

        //todo - change to lazy
        internal static IObservable<long> Intervals = null;

        internal static Random Random = new Random();

        public Game(IDrawer drawer, uint width, uint height)
        {            
            curRect.Width = width;
            curRect.Height = height;

            Intervals = Observable.Interval(TimeSpan.FromMilliseconds(TimeStep));
            _unityContainer = new UnityContainer();
            this.RegisterInUnityContainer();

            StateQueueManager = _unityContainer.Resolve<StateQueueManager>();
            Map = _unityContainer.Resolve<Map>();            
            

            loadSaveManager = new LoadSaveManager();
            loadSaveManager.LoadSnapshot(Map);

            _hero = _unityContainer.Resolve<Hero>();

            ActionRepository = _unityContainer.Resolve<IActionRepository>();

            _drawer = drawer;

            Intervals.Subscribe(StateQueueManager);
        }

        private void RegisterInUnityContainer()
        {
            _unityContainer.RegisterInstance(typeof (Hero), new Hero());
            _unityContainer.RegisterInstance(typeof(Map), new Map(curRect));
            _unityContainer.RegisterInstance(typeof (StateQueueManager), new StateQueueManager());
            _unityContainer.RegisterType(typeof(IActionRepository), typeof(ActionRepository), new ContainerControlledLifetimeManager());

            _unityContainer.RegisterTypes(
                Assembly.GetExecutingAssembly().GetTypes().Where(
                type => !type.IsAbstract && !type.IsInterface && typeof(IAction).IsAssignableFrom(type)),
                WithMappings.FromAllInterfacesInSameAssembly,//t => new[] { typeof(IAction) },
                t => t.FullName,
                WithLifetime.PerResolve);
        }

        private void LoadSettings()
        {
            Properties.Settings.Default.Reload();
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }

        public void LClick(Point visibleDestination)
        {
            var destination = Map.GetRealDestinationFromVisibleDestination(visibleDestination);
            MoveToDest(destination);
        }

        public void RClick(Point destination)
        {
            ShowActions(destination);
        }

        private void ShowActions(Point destination)
        {
            this._drawer.DrawMenu(destination.X, destination.Y, GetActions(destination));
        }

        private IEnumerable<ClientAction> GetActions(Point visibleDestination)
        {
            var destination = Map.GetRealDestinationFromVisibleDestination(visibleDestination);
            var destObject = Map.GetObjectFromDestination(destination);

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

            var possibleActions = ActionRepository.GetPossibleActions(destObject).ToList();

            var objects = new List<GameObject>(new[] {destObject});

          //  var removableObjects = objects.Select(o => this.PrepareRemovableObject(o, destination));
            return (possibleActions.SelectMany(pa =>
                {
                    return pa.GetActionsWithNecessaryObjects(objects, _hero).Select(objectsForAction =>
                    new ClientAction
                        {
                            Name = pa.GetName(objectsForAction),
                            //CanDo = pa.CanDo(_hero, objects),
                            Do = () => MoveAndDoAction(pa, destination, objectsForAction)
                        }
                    );

                }));
        }

        public void MoveToDest(Point destination)
        {
            _hero.StartMove(destination, Map.GetEasiestWay(_hero.Position, destination));
        }

        private void MoveAndDoAction(IAction action, Point destination,
            IEnumerable<GameObject> objects)
        {
            _hero.StartMove(destination, Map.GetEasiestWay(_hero.Position, destination));
            _hero.Then().StartActing(action, destination, objects);
        }

        private void DoAction(IAction action, IEnumerable<GameObject> objects)
        {
            _hero.StartActing(action, null, objects);
        }

        private RemovableWrapper<GameObject> PrepareRemovableObject(GameObject gObject, Point destination)
        {
            return new RemovableWrapper<GameObject>
            {
                GameObject = gObject,
                RemoveFromContainer = (() =>
                {
                    Map.SetObjectFromDestination(destination, null);
                })
            };
        }

        private RemovableWrapper<GameObject> PrepareRemovableObject(GameObject gObject)
        {
            return new RemovableWrapper<GameObject>
            {
                GameObject = gObject,
                RemoveFromContainer = (() =>
                {
                    _hero.RemoveFromContainer(gObject);
                })
            };
        }

        public void DrawChanges()
        {
            Map.RecalcVisibleRect(_hero.Position);

            _drawer.Clear();

            _drawer.DrawSurface(curRect.Width, curRect.Height);

            var visibleCells = Map.RectToCellRect(Map.VisibleRect);
                
            //var mapSize = _map.GetSize();
            for (int i = visibleCells.Left; i < visibleCells.Left + visibleCells.Width; i++)
            {
                for (int j = visibleCells.Top; j < visibleCells.Top+ visibleCells.Height; j++)
                {
                    var gameObject = Map.GetObjectFromCell(new Point(i, j));
                    if (gameObject != null)
                    {
                        var visibleDestination = Map.GetVisibleDestinationFromRealDestination(Map.CellToPoint(new Point(i, j)));
                        _drawer.DrawObject(gameObject.GetDrawingCode(), visibleDestination.X, visibleDestination.Y);

                    }
                }
            }

            var mobileObjects = Map.GetMobileObjects();
            foreach (var mobileObject in mobileObjects)
            {
                if (Map.PointInVisibleRect(mobileObject.Position))
                {
                    var visibleDestination = Map.GetVisibleDestinationFromRealDestination(mobileObject.Position);
                    _drawer.DrawObject(mobileObject.GetDrawingCode(), visibleDestination.X, visibleDestination.Y);
                }
            }

            _drawer.DrawHero(Map.GetVisibleDestinationFromRealDestination(_hero.Position), _hero.Angle, _hero.PointList.Select(p => Map.GetVisibleDestinationFromRealDestination(p)).ToList());

            _drawer.DrawActing(_hero.State.ShowActing);

            var groupedItems = _hero.GetContainerItems()
                .GroupBy(go => go.Name,
                    (name, gos) =>
                        new KeyValuePair<string, Func<IEnumerable<ClientAction>>>(
                            string.Format("{0}({1})", name, gos.Count()),
                            this.GetFuncForClientActions(gos.First())));

            _drawer.DrawContainer(groupedItems);

            _drawer.DrawHeroProperties(_hero.GetProperties());
        }

        private Func<IEnumerable<ClientAction>> GetFuncForClientActions(GameObject first)
        {
            return (() =>
            {
                var possibleActions = ActionRepository.GetPossibleActions(first).ToList();

                var objects = new List<GameObject>(new[] {first});

              //  var removableObjects = objects.Select(o => this.PrepareRemovableObject(o));
                return (possibleActions.SelectMany(pa =>
                {
                    return pa.GetActionsWithNecessaryObjects(objects, _hero).Select(objectsForAction =>
                    new ClientAction
                    {
                        Name = pa.GetName(objectsForAction),
                        //CanDo = pa.CanDo(_hero, objects),
                        Do = () => DoAction(pa, objectsForAction)
                    }
                    );

                }));
            });
        }
    }
}
