using System;
using System.Collections.Generic;
using System.Windows;
using Game.Engine.Interfaces;
using Microsoft.Practices.Unity;

namespace Game.Engine.Objects
{
    internal class ObjectWithState: IComparable<ObjectWithState>
    {
        // todo : object with state
        // to switch state objects are added to quiue in appropriate game tick
        // when appropriate tick is now - state of objects is changing
        // properties of object within one state is calculated when object interacts with hero
        // when state of object changes not by shedule - StateQueueManager searches for object and remove from queue and place it in another appropriate queue
        // game ticks to next state should be calculated with random distribution

        // todo : maybe rewrite with empty ctor and virtual methods for state registration

        private readonly Action NextStateHandler;
        public readonly Action LastStateHandler;

        public ObjectWithState(List<IObjectState> objectStateQueue, bool isCircling, Action lastStateHandler, Action nextStateHandler)
        {
            _objectStateQueue = objectStateQueue;
            _isCircling = isCircling;

            LastStateHandler = lastStateHandler;
            NextStateHandler = nextStateHandler;

            NextState();
        }

        public ObjectWithState(List<IObjectState> objectStateQueue, bool isCircling, Action lastStateHandler)
            :this(objectStateQueue, isCircling, lastStateHandler, null)
        {}

        public int NextStateTick { get; set; }
        public int TicksToNextState {
            get
            {
                if (Game.StateQueueManager != null)
                {
                    return NextStateTick - Game.StateQueueManager.CurrentTick;
                }

                return 0;
            }
        }

        protected readonly List<IObjectState> _objectStateQueue;
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

        public virtual void NextState()
        {
            var nextStateId = _currentStateId;
            nextStateId++;
            if (nextStateId >= _objectStateQueue.Count)
            {
                if (!_isCircling)
                {
                    _currentStateId = nextStateId;
                    OnLastStateFinished();
                    return;
                }

                nextStateId = 0;
            }

            ChangeState(nextStateId);
        }

        public virtual void ChangeState(int newstateId, int? newTicksCount = null)
        {
            var oldState = (_currentStateId >= _objectStateQueue.Count || _currentStateId < 0) ? null :_objectStateQueue[_currentStateId];
            _currentStateId = newstateId;

            _objectStateQueue[_currentStateId].TickCount = newTicksCount ?? _objectStateQueue[_currentStateId].TickCount;

            if (Game.StateQueueManager != null)
            {
                Game.StateQueueManager.AddObjectToQueue(_objectStateQueue[_currentStateId].TickCount, _objectStateQueue[_currentStateId].Distribution, this);
                OnChangeState(oldState);
            }
        }

        protected virtual void OnChangeState(IObjectState oldState)
        {
            if (NextStateHandler != null)
                NextStateHandler();
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

        public virtual void OnLastStateFinished()
        {
            if (LastStateHandler != null)
                LastStateHandler();
        }
    }
}
