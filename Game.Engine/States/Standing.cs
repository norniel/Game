using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Engine
{
    class Standing : IState
    {
        private Hero _hero;

        public Standing( Hero hero )
        {
            _hero = hero;
        }

        #region IState Members

        //public event StateHandler NextState;

        public void Act()
        {
        }

        #endregion
    }
}
