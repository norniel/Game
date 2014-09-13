using System;

namespace Game.Engine
{
    [Flags]
    public enum Property
    {
        Pickable = 1,
        Cuttable = 2,
        Collectable = 3,
        CollectBerries = 4,
        CollectBranch = 5,
        Eatable = 6,
        Dropable,
        Cutter,
        NeedToCreateStoneAxe
    }
}
