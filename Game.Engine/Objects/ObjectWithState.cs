using System;
using System.Collections.Generic;
using System.Windows;
using Game.Engine.Interfaces;
using Microsoft.Practices.Unity;

namespace Game.Engine.Objects
{
    /*namespace MyNamespace
    {
        interface IStateManager{
            void ClearEvents(State value);
            void AddEvents(State value);
        }

        class StateManager : IStateManager
        {
            public void ClearEvents(State value)
            {
                IEnumerable<Trigger> trigger = value.GetAllTriggers();
            }

            public void AddEvents(State value)
            {
                throw new NotImplementedException();
            }
        }


        class ObjectWithState<TState> where TState:State
        {
            private TState _currentSate;
            protected IStateManager StateManager { get; set; }

            public TState CurrentSate
            {
                get { return _currentSate; }
                protected set
                {
                    StateManager.ClearEvents(value);
                    _currentSate = value;
                    StateManager.AddEvents(value);
                }
            }
        }

        class Fire : ObjectWithState<FireState>
        {
            

            public Fire()
            {
                CurrentSate = new FlameState(this, 10);
            }


            public void Погасни()
            {
                
            }
        }


        abstract class State
        {

        }

        class FireState:State { }

        class FlameState : FireState
        {
            private readonly Fire _fire;
            private readonly int _power;

            public FlameState(Fire fire, int power)
            {
                _fire = fire;
                _power = power;
            }


            IEnumerable<Trigger> Triggers
            {
                get { yield return new Погаснуть(_fire, _power); }
            }

        }

        class FlameVanishState : FireState { }


    
    }



    */

    internal abstract class ObjectWithState: FixedObject, IComparable<ObjectWithState>
    {

        // todo : object with state
        // to switch state objects are added to quiue in appropriate game tick
        // when appropriate tick is now - state of objects is changing
        // properties of object within one state is calculated when object interacts with hero
        // when state of object changes not by shedule - StateQueueManager searches for object and remove from queue and place it in another appropriate queue
        // game ticks to next state should be calculated with random distribution

        // todo : maybe rewrite with empty ctor and virtual methods for state registration
        public ObjectWithState(List<IObjectState> objectStateQueue, bool isCircling)
        {
            _objectStateQueue = objectStateQueue;
            _isCircling = isCircling;
            
            ChangeState();
        }

        public int NextStateTick { get; set; }

        private readonly List<IObjectState> _objectStateQueue;
        private readonly bool _isCircling;
        private int _currentStateId = -1;

        public IObjectState CurrentState {
            get
            {
                if (_currentStateId < 0 || _currentStateId >= _objectStateQueue.Count)
                    return null;

                return _objectStateQueue[_currentStateId];
            }
        }

        public virtual void ChangeState()
        {
            _currentStateId++;
            if (_currentStateId >= _objectStateQueue.Count)
            {
                if (!_isCircling)
                {
                    return;
                }

                _currentStateId = 0;
            }

            if (Game.StateQueueManager != null)
            {
                Game.StateQueueManager.AddObjectToQueue(_objectStateQueue[_currentStateId].TickCount, _objectStateQueue[_currentStateId].Distribution, this);
            }
        }

        public int CompareTo(ObjectWithState other)
        {
            if (NextStateTick.CompareTo(other.NextStateTick) != 0)
            {
                return NextStateTick.CompareTo(other.NextStateTick);
            }

            // todo important!!!!!! replace with id or rewrite GetHashCode!!!!
            return this.GetHashCode().CompareTo(other.GetHashCode());
        }
    }
}
/*
namespace Game.Engine.Objects.MyNamespace
{
    interface ITrigger
    {
         
    }

    internal class Погаснуть : ITrigger
    {
        private readonly Fire _fire;

        public Погаснуть(Fire fire, int timer)
        {
            _fire = fire;
        }

        void Do()
        {
            _fire.Погасни();
        }

    }
}*/
