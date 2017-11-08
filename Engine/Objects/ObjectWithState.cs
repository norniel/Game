﻿using System;
using System.Collections.Generic;
using System.Threading;
using Engine.Interfaces;

namespace Engine.Objects
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

        private static int _idCounter;

        private static int GenerateId()
        {
            return Interlocked.Increment(ref _idCounter);
        }


        public ObjectWithState(List<IObjectState> objectStateQueue, bool isCircling, Action lastStateHandler, Action nextStateHandler)
        {
            _id = GenerateId();
            ObjectStateQueue = objectStateQueue;
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

        protected readonly List<IObjectState> ObjectStateQueue;
        private readonly bool _isCircling;
        private int _currentStateId = -1;

        private readonly int _id;

        public IObjectState CurrentState {
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

        protected virtual void OnChangeState(IObjectState oldState)
        {
            NextStateHandler?.Invoke();
        }

        public int CompareTo(ObjectWithState other)
        {
            if (NextStateTick.CompareTo(other.NextStateTick) != 0)
            {
                return NextStateTick.CompareTo(other.NextStateTick);
            }

            return _id.CompareTo(other._id);
        }

        public virtual void OnLastStateFinished()
        {
            LastStateHandler?.Invoke();
        }
    }
}
