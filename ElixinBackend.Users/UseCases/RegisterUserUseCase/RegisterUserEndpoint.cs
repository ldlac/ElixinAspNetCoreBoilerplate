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
            endpoints.MapPost<RegisterUserCommand>("/register", async (serviceProvider, registerUserCommand, response) =>
            {
                var mediator = serviceProvider.GetRequiredService<IMediator>();

                var user = await mediator.Send(registerUserCommand);

                await response.Created(user);
            });

            return endpoints;
        }
    }
}