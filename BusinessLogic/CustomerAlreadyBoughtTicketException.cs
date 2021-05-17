using System;
using System.Runtime.Serialization;

namespace BusinessLogic
{
    [Serializable]
    public class CustomerAlreadyBoughtTicketException : Exception
    {
        public CustomerAlreadyBoughtTicketException()
        {
        }

        public CustomerAlreadyBoughtTicketException(string message) : base(message)
        {
        }

        public CustomerAlreadyBoughtTicketException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CustomerAlreadyBoughtTicketException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}