using System;
using System.Runtime.Serialization;

namespace BusinessLogic
{
    [Serializable]
    public class WasntActivatedByAirlineException : Exception
    {
        public WasntActivatedByAirlineException()
        {
        }

        public WasntActivatedByAirlineException(string message) : base(message)
        {
        }

        public WasntActivatedByAirlineException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WasntActivatedByAirlineException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}