using System;
using System.Runtime.Serialization;

namespace BusinessLogic
{
    [Serializable]
    internal class OutOfTicketsExecption : Exception
    {
        public OutOfTicketsExecption()
        {
        }

        public OutOfTicketsExecption(string message) : base(message)
        {
        }

        public OutOfTicketsExecption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OutOfTicketsExecption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}