namespace Engine.Tools
{
    public class BurningProps
    {
        public Point Point { get; }

        public int LightRadius { get; }

        public BurningProps(Point point, int lightRadius)
        {
            Point = point;
            LightRadius = lightRadius;
        }
    }
}
