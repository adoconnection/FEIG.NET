using System;
using System.Runtime.Serialization;

namespace FeigDotNet.Exceptions
{
    public class FeigConnectionException : FeigException
    {
        public FeigConnectionException()
        {
        }

        public FeigConnectionException(string message) : base(message)
        {
        }

        public FeigConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FeigConnectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}