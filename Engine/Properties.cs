using System;

namespace Engine
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
        NeedToCreateStoneAxe,
        NeedToCreateFire,
        Burning,
        Regrowable,
        Roastable,
        CollectTwig,
        NeedToBuildGrassBed,
        NeedToSleep,
        NeedToBuildWickiup,
        Digger,
        Stone,
        Branch,
        Diggable,
        CollectRoot,
        Enterable
    }
}
