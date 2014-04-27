using System;

namespace Game.Engine.BridgeObjects
{
    public class ClientAction
    {
        public string Name { get; set; }

        public bool CanDo { get; set; }

        public Action Do { get; set; }
    }
}
