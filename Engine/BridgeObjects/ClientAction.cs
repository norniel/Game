using System;

namespace Engine.BridgeObjects
{
    public class ClientAction
    {
        public string Name { get; set; }

        public bool CanDo {
            get { return true; }
        }

        public Action Do { get; set; }
    }
}
