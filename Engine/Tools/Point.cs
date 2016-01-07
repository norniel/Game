using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class Point
    {
        public int X { get;  set; }
        public int Y { get; set; }


        public Point()
        {
            X = 0;
            Y = 0;
        }

        public Point( int x, int y) 
        {
            X = x;
            Y = y;
        }

        public Point( Point point )
        {
            X = point.X;
            Y = point.Y;
        }

        public bool Equals(Point other)
        {
              return other.X == X && other.Y == Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (Point)) return false;
            return Equals((Point) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode()*397) ^ Y.GetHashCode();
            }
        }

        public static bool operator ==(Point left, Point right)
        {
            return !ReferenceEquals(null, right) && left.Equals(right);
        }

        public static bool operator !=(Point left, Point right)
        {
            if (ReferenceEquals(null, left))
                return !ReferenceEquals(null, right);

            if (ReferenceEquals(null, right))
                return true;

            return !left.Equals(right);
        }

        public static Point operator +(Point left, Point right)
        {
            return new Point( left.X + right.X, left.Y + right.Y);
        }

        public override string ToString()
        {
            return string.Format("[{0};{1}]", X, Y);
        }
    }
}
