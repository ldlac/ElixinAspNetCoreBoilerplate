using ElixinBackend.Users.UseCases.AuthenticateUseCase;
using ElixinBackend.Users.UseCases.RegisterUserUseCase;
using Microsoft.AspNetCore.Routing;

namespace ElixinBackend.Users
{
    public static class Endpoints
    {
        public static IEndpointRouteBuilder MapUsers(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapRegisterUser()
                .MapGetUserBy()
                .MapAuthenticate();
        }
    }
}