using System;
using System.Collections.Generic;
using System.Linq;
using Engine.ObjectStates;

namespace Engine.Objects
{
    public class ObjectWithState: ObjectWithStateBase
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

        public override int NextStateTick
        {
            get => _nextStateTick;
            set
            {
                if (CurrentState?.Eternal ?? false)
                {
                    _nextStateTick = int.MaxValue;
                }
                else
                    _nextStateTick = value;
            } 
        }


        public ObjectWithState(List<ObjectState> objectStateQueue, bool isCircling, Action lastStateHandler, Action nextStateHandler = null, bool isOffOnStart = false)
        {
            ObjectStateQueue = objectStateQueue;
            _isCircling = isCircling;

            LastStateHandler = lastStateHandler;
            NextStateHandler = nextStateHandler;

            if (isOffOnStart)
            {
                _currentStateId = ObjectStateQueue.Count;
            }

            NextState();
        }

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

        protected List<ObjectState> ObjectStateQueue;
        private readonly bool _isCircling;
        private int _currentStateId = -1;
        private int _nextStateTick;

        public ObjectState CurrentState {
            get
            {
                if (_currentStateId < 0 || _currentStateId >= ObjectStateQueue.Count)
                    return null;

                return ObjectStateQueue[_currentStateId];
            }
        }

        public virtual void NextState()
        {
            if (_currentStateId >= ObjectStateQueue.Count)
                return;

            var nextStateId = _currentStateId;
            nextStateId++;
            if (nextStateId >= ObjectStateQueue.Count)
            {
                if (!_isCircling)
                {
                    _currentStateId = nextStateId;
                    OnLastStateFinished();
                    return;
                }

                nextStateId = 0;
            }

            ChangeStateInternal(nextStateId);
        }

        internal bool HasState(ObjectStates.ObjectStates stateName)
        {
            return ObjectStateQueue.Any(objState => objState.Name == stateName);
        }

        public virtual void ChangeState(int newstateId, int? newTicksCount = null)
        {
            Game.StateQueueManager.RemoveObjectFromQueue(this);
            ChangeStateInternal(newstateId, newTicksCount);
        }

        protected virtual void ChangeStateInternal(int newstateId, int? newTicksCount = null)
        {
            var oldState = (_currentStateId >= ObjectStateQueue.Count || _currentStateId < 0) ? null :ObjectStateQueue[_currentStateId];
            _currentStateId = newstateId;

            ObjectStateQueue[_currentStateId].TickCount = newTicksCount ?? ObjectStateQueue[_currentStateId].TickCount;

            if (Game.StateQueueManager != null)
            {
                Game.StateQueueManager.AddObjectToQueue(ObjectStateQueue[_currentStateId].TickCount, ObjectStateQueue[_currentStateId].Distribution, this);
                OnChangeState(oldState);
            }
        }

        protected virtual void OnChangeState(ObjectState oldState)
        {
            NextStateHandler?.Invoke();
        }

        public virtual void OnLastStateFinished()
        {
            LastStateHandler?.Invoke();
        }

        public void ChangeStateList(List<ObjectState> objectStates)
        {
            if (objectStates.Count <= 0)
                return;

            ObjectStateQueue = objectStates;
            ChangeState(0);
        }
    }
}
