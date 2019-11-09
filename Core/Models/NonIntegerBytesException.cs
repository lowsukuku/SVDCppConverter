using System;
using System.Runtime.Serialization;

namespace Core.Models
{
    [Serializable]
    public class NonIntegerBytesException : Exception
    {
        public NonIntegerBytesException()
        { }

        public NonIntegerBytesException(string message) : base(message)
        { }

        public NonIntegerBytesException(string message, Exception innerException) : base(message, innerException)
        { }

        protected NonIntegerBytesException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
