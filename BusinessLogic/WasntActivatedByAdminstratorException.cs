using System;
using System.Runtime.Serialization;

namespace BusinessLogic
{
    [Serializable]
    internal class WasntActivatedByAdminstratorException : Exception
    {
        public WasntActivatedByAdminstratorException()
        {
        }

        public WasntActivatedByAdminstratorException(string message) : base(message)
        {
        }

        public WasntActivatedByAdminstratorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WasntActivatedByAdminstratorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}