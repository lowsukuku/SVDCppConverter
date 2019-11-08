using System;

namespace Core.Models
{
    public class Offset
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

        public static Offset FromBits(int bits) => new Offset(bits);
        public static Offset FromBytes(int bytes) => new Offset(bytes * 8);

        public static Offset operator +(Offset left, Offset right) => new Offset(left.Bits + right.Bits);
        public static Offset operator -(Offset left, Offset right) => new Offset(left.Bits - right.Bits);

        public static bool operator <(Offset left, Offset right)
        {
            if (left is null || right is null)
                return false;
            return left.Bits < right.Bits;
        }

        public static bool operator >(Offset left, Offset right)
        {
            return right < left;
        }

        public static bool operator ==(Offset left, Offset right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }
        public static bool operator !=(Offset left, Offset right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Offset;
            if (other is null)
                return false;

            return Equals(other);
        }

        protected bool Equals(Offset other)
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


        private Offset(int bits)
        {
            Bits = bits;
        }
    }
}
