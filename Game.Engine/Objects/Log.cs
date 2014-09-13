using System.Collections.Generic;
using Game.Engine.Interfaces;
using Game.Engine.Objects.Fruits;

namespace Game.Engine.Objects.Trees
{
    class Log : FixedObject
    {
        public Log()
        {
            Id = 0x00001400;
            IsPassable = true;
            Size = new Size(1, 1);
        }

        public override string Name
        {
            get { return "Log"; }
        }
    }
}