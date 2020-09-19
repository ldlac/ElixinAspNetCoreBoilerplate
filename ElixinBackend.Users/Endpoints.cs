using ElixinBackend.Users.UseCases.AuthenticateUseCase;
using ElixinBackend.Users.UseCases.RegisterUserUseCase;
using Microsoft.AspNetCore.Routing;

namespace ElixinBackend.Users
{
    public static class Endpoints
    {
        public static void MapUsers(this IEndpointRouteBuilder endpoints)
        {
            endpoints
                .MapRegisterUser()
                .MapGetUserBy()
                .MapAuthenticate();
        }
    }
}