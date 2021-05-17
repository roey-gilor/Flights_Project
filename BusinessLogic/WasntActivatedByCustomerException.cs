using System;
using System.Runtime.Serialization;

namespace BusinessLogic
{
    [Serializable]
    public class WasntActivatedByCustomerException : Exception
    {
        public WasntActivatedByCustomerException()
        {
        }

        public WasntActivatedByCustomerException(string message) : base(message)
        {
        }

        public WasntActivatedByCustomerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WasntActivatedByCustomerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}