using ElixinBackend.Utils;
using MediatR;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace ElixinBackend.Users.UseCases.RegisterUserUseCase
{
    public static class RegisterUserEndpoint
    {
        public static IEndpointRouteBuilder MapRegisterUser(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost<RegisterUser>("/register", async (serviceProvider, registerUser, response) =>
            {
                var mediator = serviceProvider.GetRequiredService<IMediator>();

                var command = new RegisterUserCommand() { Username = registerUser.Username, Password = registerUser.Password };

                var user = await mediator.Send(command);

                await response.Created(user);
            });

            return endpoints;
        }
    }
}