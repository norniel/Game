﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public interface IState
    {
       // event StateHandler NextState;

        void Act();

        bool ShowActing { get; }
    }

    public class StateEvent
    {
        public event StateHandler NextState;

        public void FireEvent()
        {
            if (NextState != null)
                NextState(null, new StateEventArgs());
        }
    }

    public delegate void StateHandler(object sender, StateEventArgs stateEventArgs);

    public class StateEventArgs : EventArgs
    {
        private IState state;
        public IState State
        {
            get { return state; }
            set { state = value; }
        }
    }
}
