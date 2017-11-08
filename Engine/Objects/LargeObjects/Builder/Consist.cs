using System.Collections.Generic;

namespace Engine.Objects.LargeObjects.Builder
{
    class Consist
    {
       internal List<List<Dictionary<int, GameObject>>> steps = new List<List<Dictionary<int, GameObject>>>();

        public List<List<Dictionary<int, GameObject>>> Steps => steps;
    }
}
