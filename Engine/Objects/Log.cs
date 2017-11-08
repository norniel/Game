using System.Collections.Generic;
using Engine.Interfaces;
using Engine.Objects.Fruits;

namespace Engine.Objects.Trees
{
    class Log : FixedObject
    {
        public Log()
        {
            Id = 0x00001400;
            IsPassable = true;
            Size = new Size(1, 1);
        }

        public override string Name => "Log";
    }
}