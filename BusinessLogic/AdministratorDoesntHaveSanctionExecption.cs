using System;
using System.Runtime.Serialization;

namespace BusinessLogic
{
    [Serializable]
    internal class AdministratorDoesntHaveSanctionExecption : Exception
    {
        public AdministratorDoesntHaveSanctionExecption()
        {
        }

        public AdministratorDoesntHaveSanctionExecption(string message) : base(message)
        {
        }

        public AdministratorDoesntHaveSanctionExecption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AdministratorDoesntHaveSanctionExecption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}