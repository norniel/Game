using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Engine
{
    public struct Rect
    {
        public int Left{get;set;}
        public int Top{get;set;}
        public uint Width { get; set; }
        public uint Height { get; set; }

        public Rect( int left, int top, uint width, uint height ) : this()
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public bool Equals(Rect other)
        {
            return other.Left == Left && other.Top == Top && other.Width == Width && other.Height == Height;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (Rect)) return false;
            return Equals((Rect) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Left;
                result = (result*397) ^ Top;
                result = (result*397) ^ Width.GetHashCode();
                result = (result*397) ^ Height.GetHashCode();
                return result;
            }
        }

        public static bool operator ==(Rect left, Rect right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Rect left, Rect right)
        {
            return !left.Equals(right);
        }
    }
}
