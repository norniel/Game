using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Engine
{
    public interface IState
    {
        event StateHandler NextState;

        void Act();
    }

    public delegate void StateHandler(IState state);
}
