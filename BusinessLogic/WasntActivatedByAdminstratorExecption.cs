using System;
using System.Runtime.Serialization;

namespace BusinessLogic
{
    [Serializable]
    internal class WasntActivatedByAdminstratorExecption : Exception
    {
        public WasntActivatedByAdminstratorExecption()
        {
        }

        public WasntActivatedByAdminstratorExecption(string message) : base(message)
        {
        }

        public WasntActivatedByAdminstratorExecption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WasntActivatedByAdminstratorExecption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}