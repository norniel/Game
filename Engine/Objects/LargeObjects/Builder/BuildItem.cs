using System;
using System.Collections.Generic;

namespace Engine.Objects.LargeObjects.Builder
{
    public class BuildItem
    {
        public Func<GameObject, bool> CheckObject { get; protected set; }
        public Func<List<GameObject>, bool> CheckObjects { get; protected set; }
        public int PercentPerItem { get; protected set; }
        public int CountUsedToBuild { get; set; }
    }
}
