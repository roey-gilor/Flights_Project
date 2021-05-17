using System;
using System.Runtime.Serialization;

namespace BusinessLogic
{
    [Serializable]
    public class OutOfTicketsException : Exception
    {
        public OutOfTicketsException()
        {
        }

        public OutOfTicketsException(string message) : base(message)
        {
        }

        public OutOfTicketsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OutOfTicketsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}