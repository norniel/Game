using System;

namespace Engine.Tools
{
    public class Vector
    {
        public double X { get; protected set; }
        public double Y { get; protected set; }


        public Vector()
        {
            X = 0;
            Y = 0;
        }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector(Vector vector)
        {
            X = vector.X;
            Y = vector.Y;
        }

        public bool Equals(Vector other)
        {
            return other.X == X && other.Y == Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Vector)) return false;
            return Equals((Vector)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public static bool operator ==(Vector left, Vector right)
        {
            if (ReferenceEquals(null, left))
                return ReferenceEquals(null, right);

            return !ReferenceEquals(null, right) && left.Equals(right);
        }

        public static bool operator !=(Vector left, Vector right)
        {
            if (ReferenceEquals(null, left))
                return !ReferenceEquals(null, right);

            if (ReferenceEquals(null, right))
                return true;

            return !left.Equals(right);
        }

        public static Vector operator +(Vector left, Vector right)
        {
            return new Vector(left.X + right.X, left.Y + right.Y);
        }

        public override string ToString()
        {
            return $"[{X};{Y}]";
        }

        public static Vector FromPoints(Point position, Point cellToPoint)
        {
            return new Vector(cellToPoint.X-position.X, cellToPoint.Y-position.Y);

        }

        public Vector Normalize()
        {
            var distance = Math.Sqrt(X * X + Y * Y);

            return distance < 0.000001 ? new Vector(0,0):  new Vector(X/distance, Y/distance);
        }

        public double Angle()
        {
            if (Math.Abs(X) >= 0.0001)
                return 180 * Math.Atan(Y / X) / Math.PI + (X <= 0 ? 180 : 0);
            else
                return Y >= 0 ? 90 : 270;
        }

        public Vector TurnByAngle(int angle)
        {
            //x2 = cosβx1−sinβy1 y2 = sinβx1 + cosβy1
            return new Vector(Math.Cos(angle)*X-Math.Sin(angle)*Y, Math.Sin(angle) * X + Math.Cos(angle) * Y);
        }

        public Vector Multiply(double i)
        {
            return new Vector(i * X, i * Y);
        }
    }
}
