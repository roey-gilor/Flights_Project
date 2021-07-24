using System;
using System.Runtime.Serialization;

namespace BusinessLogic
{
    [Serializable]
    public class DuplicateDetailsException : Exception
    {
        public DuplicateDetailsException()
        {
        }

        public DuplicateDetailsException(string message) : base(message)
        {
        }

        public DuplicateDetailsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicateDetailsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}