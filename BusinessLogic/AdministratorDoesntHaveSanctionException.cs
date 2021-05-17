using System;
using System.Runtime.Serialization;

namespace BusinessLogic
{
    [Serializable]
    public class AdministratorDoesntHaveSanctionException : Exception
    {
        public AdministratorDoesntHaveSanctionException()
        {
        }

        public AdministratorDoesntHaveSanctionException(string message) : base(message)
        {
        }

        public AdministratorDoesntHaveSanctionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AdministratorDoesntHaveSanctionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}