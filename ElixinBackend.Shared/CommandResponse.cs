using System;
using System.Threading.Tasks;

namespace ElixinBackend.Shared
{
    public class CommandResponse<T>
    {
        private readonly T Entity;
        private readonly string Error;

        public static CommandResponse<T> FromSuccess(T entity)
        {
            return new CommandResponse<T>(entity, null);
        }

        public static CommandResponse<T> FromFailure(string error)
        {
            return new CommandResponse<T>(error);
        }

        public static CommandResponse<T> FromFailure(string error, T entity)
        {
            return new CommandResponse<T>(entity, error);
        }

        private CommandResponse(string error)
        {
            Entity = default;
            Error = error;
        }

        private CommandResponse(T entity, string error)
        {
            Entity = entity;
            Error = error;
        }

        public async Task Resolve(Func<T, Task> OnSuccess = null, Func<string, T, Task> OnFailure = null)
        {
            if (Error is null)
            {
                await (OnSuccess?.Invoke(Entity) ?? Task.CompletedTask);
            }
            else
            {
                await (OnFailure?.Invoke(Error, Entity) ?? Task.CompletedTask);
            }
        }
    }
}