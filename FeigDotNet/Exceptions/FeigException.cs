using System;
using System.Runtime.Serialization;

namespace FeigDotNet.Exceptions
{
    public class FeigException : Exception
    {
        public FeigException()
        {
        }

        public FeigException(string message) : base(message)
        {
        }

        public FeigException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FeigException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}