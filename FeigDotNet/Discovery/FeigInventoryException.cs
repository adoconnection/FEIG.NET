using System;
using System.Runtime.Serialization;
using FeigDotNet.Exceptions;

namespace FeigDotNet.Discovery
{
    public class FeigInventoryException : FeigException
    {
        private readonly int errorCode;
        public int ErrorCode { get; set; }

        public FeigInventoryException()
        {
        }

        public FeigInventoryException(int errorCode, string message) : base(message)
        {
            this.errorCode = errorCode;
        }

        public FeigInventoryException(string message) : base(message)
        {
        }

        public FeigInventoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FeigInventoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}