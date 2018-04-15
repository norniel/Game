using Engine.Interfaces;

namespace Engine.Behaviors
{
    internal class BurnableBehavior : IBehavior
    {
        //int Humidity { get; set; }
        //int V{get; set;}
        //int Ro{get;set;}
        //int Teploprovodnost { get; set; }

        public BurnableBehavior(int burnableCoefficient)
        {
            Сoefficient = burnableCoefficient;
        }

        public int Сoefficient { get; private set; }
    }
}
