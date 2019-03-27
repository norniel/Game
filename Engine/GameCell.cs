using System.Collections.Generic;

namespace Engine
{
    public class GameCell
    {
        public FixedObject FixedObject { get; set; }
        public List<MobileObject> MobileList { get; set; }
    }
}
