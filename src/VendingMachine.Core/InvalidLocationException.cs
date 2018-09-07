using System;
using System.Runtime.Serialization;

namespace VendingMachine.Core
{
    [Serializable]
    public class InvalidLocationException : Exception
    {
        public InvalidLocationException(string location) : base($"{location} does not exist")
        {
        }

        public InvalidLocationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidLocationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}