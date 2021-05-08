using System;
using System.Runtime.Serialization;

namespace BusinessLogic
{
    [Serializable]
    internal class WasntActivatedByUserException : Exception
    {
        public WasntActivatedByUserException()
        {
        }

        public WasntActivatedByUserException(string message) : base(message)
        {
        }

        public WasntActivatedByUserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WasntActivatedByUserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}