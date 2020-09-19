using System;
using System.Runtime.Serialization;

namespace ElixinBackend.Users.UseCases.GetUserUseCase
{
    [Serializable]
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("USER.NOT.FOUND")
        {
        }

        public UserNotFoundException(string message) : base(message)
        {
        }

        public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}