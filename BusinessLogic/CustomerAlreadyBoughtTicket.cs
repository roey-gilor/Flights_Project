using System;
using System.Runtime.Serialization;

namespace BusinessLogic
{
    [Serializable]
    internal class CustomerAlreadyBoughtTicket : Exception
    {
        public CustomerAlreadyBoughtTicket()
        {
        }

        public CustomerAlreadyBoughtTicket(string message) : base(message)
        {
        }

        public CustomerAlreadyBoughtTicket(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CustomerAlreadyBoughtTicket(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}