﻿using Engine.Interfaces;
using Engine.Objects;

namespace Engine.Behaviors
{
    class RoastBehavior : IBehavior
    {
        private readonly string _name;
        public RoastBehavior(string roastedName)
        {
            _name = roastedName;
        }

        public GameObject GetRoasted()
        {
            return Game.Factory.Produce(_name);
        }
    }
}
