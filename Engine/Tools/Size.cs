using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public struct Size
    {
        public uint Width { get; set; }
        public uint Height { get; set; }

        public Size( uint width, uint height): this()
        {
            Width = width;
            Height = height;
        }

        public bool Equals(Size other)
        {
            return other.Width == Width && other.Height == Height;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (Size)) return false;
            return Equals((Size) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Width.GetHashCode()*397) ^ Height.GetHashCode();
            }
        }

        public static bool operator ==(Size left, Size right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Size left, Size right)
        {
            return !left.Equals(right);
        }
    }
}
