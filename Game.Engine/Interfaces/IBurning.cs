namespace Game.Engine.Interfaces
{
    interface IBurning
    {
        int TimeOfBurning { get; set; }

        //int Temperature {get; set;}

        int LightRadius { get; }
    }

    public class BurningProps
    {
        public Point Point { get; private set; }

        public int LightRadius { get; private set; }

        public BurningProps(Point point, int lightRadius)
        {
            Point = point;
            LightRadius = lightRadius;
        }
    }
}
