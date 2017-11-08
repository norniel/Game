namespace Engine.Heros
{
    internal class HeroProperties
    {
        public double _innerTiredness;
        public int Satiety { get; set; }

        public int Health { get; set; }

        public int Tiredness { get; private set; }

        public double InnerTiredNess {
            get => _innerTiredness;
            set
            {
                _innerTiredness = value;
                Tiredness = (int)_innerTiredness;
            }
        }
    }
}
