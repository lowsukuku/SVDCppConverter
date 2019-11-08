using System;

namespace Core.Models
{
    public class Width
    {
        public int Bytes
        {
            get
            {
                if (Bits % 8 != 0)
                    throw new NonIntegerBytesException();
                return Bits / 8;
            }
        }

        public int Bits { get; }

        public static Width FromBits(int bits) => new Width(bits);
        public static Width FromBytes(int bytes) => new Width(bytes * 8);

        public static Width operator +(Width left, Width right) => new Width(left.Bits + right.Bits);
        public static Width operator -(Width left, Width right) => new Width(left.Bits - right.Bits);

        public static bool operator ==(Width left, Width right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }
        public static bool operator !=(Width left, Width right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Width;
            if (other is null)
                return false;

            return Equals(other);
        }

        protected bool Equals(Width other)
        {
            return Bits == other.Bits;
        }

        public override int GetHashCode()
        {
            return Bits.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Bits} bits";
        }

        private Width(int bits)
        {
            Bits = bits;
        }
    }
}
