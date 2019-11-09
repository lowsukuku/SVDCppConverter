namespace Core.Models
{
    public abstract class BitsAndBytesRepresentable<T> where T : BitsAndBytesRepresentable<T>, new()
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

        public int Bits { get; private set; }

        public static T FromBits(int bits) => new T { Bits = bits };
        public static T FromBytes(int bytes) => new T { Bits = bytes * 8 };

        public static T operator +(BitsAndBytesRepresentable<T> left, BitsAndBytesRepresentable<T> right) => new T { Bits = left.Bits + right.Bits };
        public static T operator -(BitsAndBytesRepresentable<T> left, BitsAndBytesRepresentable<T> right) => new T { Bits = left.Bits - right.Bits };

        public static bool operator <(BitsAndBytesRepresentable<T> left, BitsAndBytesRepresentable<T> right)
        {
            if (left is null || right is null)
                return false;
            return left.Bits < right.Bits;
        }

        public static bool operator >(BitsAndBytesRepresentable<T> left, BitsAndBytesRepresentable<T> right)
        {
            return right < left;
        }

        public static bool operator ==(BitsAndBytesRepresentable<T> left, BitsAndBytesRepresentable<T> right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }
        public static bool operator !=(BitsAndBytesRepresentable<T> left, BitsAndBytesRepresentable<T> right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            var other = obj as BitsAndBytesRepresentable<T>;
            if (other is null)
                return false;

            return Equals(other);
        }

        protected bool Equals(BitsAndBytesRepresentable<T> other)
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


        protected BitsAndBytesRepresentable()
        { }
    }
}
