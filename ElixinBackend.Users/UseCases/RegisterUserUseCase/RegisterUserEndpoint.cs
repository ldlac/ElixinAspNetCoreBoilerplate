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

                await (await mediator.Send(registerUserCommand))
                    .Resolve(OnSuccess: async (user) =>
                    {
                        await response.Created(user);
                    },
                    OnFailure: async (error, user) =>
                    {
                        switch (error)
                        {
                            case RegisterUserCommandException.UsernameAlreadyExists:
                                await response.BadRequest(new { Message = error });
                                break;
                        }
                    });
            });

            return endpoints;
        }
    }
}