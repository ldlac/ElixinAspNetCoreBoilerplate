using System;
using System.Runtime.Serialization;

namespace ElixinBackend.Users.Services
{
    [Serializable]
    public class AuthentificationFailedException : Exception
    {
        public AuthentificationFailedException() : base("AUTHENTIFICATION.FAILED")
        {
        }

        public AuthentificationFailedException(string message) : base(message)
        {
        }

        public AuthentificationFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AuthentificationFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}