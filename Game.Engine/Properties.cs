using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Engine
{
    [Flags]
    public enum Property
    {
        Pickable = 1,
        Cuttable = 2,
        Collectable = 3,
        CollectBerries = 4,
        CollectBranch
    }
}
