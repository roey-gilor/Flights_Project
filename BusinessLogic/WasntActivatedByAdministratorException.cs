using System;
using System.Runtime.Serialization;

namespace BusinessLogic
{
    [Serializable]
    internal class WasntActivatedByAdministratorException : Exception
    {
        public WasntActivatedByAdministratorException()
        {
        }

        public WasntActivatedByAdministratorException(string message) : base(message)
        {
        }

        public WasntActivatedByAdministratorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WasntActivatedByAdministratorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}