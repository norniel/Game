using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Engine
{
    class Cell 
    {
        public uint Number { get; private set; }

        public Cell()
        {
            Number = 0x00000000;
        }
    }
}
