using ElixinBackend.Users.UseCases.GetUserUseCase;
using ElixinBackend.Utils;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace ElixinBackend.Users.UseCases.RegisterUserUseCase
{
    public static class GetUserByEndpoint
    {
        public static IEndpointRouteBuilder MapGetUserBy(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/users/{username}", async (context) =>
            {
                var username = context.Request.RouteValues["username"] as string;

                var mediator = context.RequestServices.GetRequiredService<IMediator>();

                await (await mediator.Send(new FindUserByUsernameCommand() { Username = username }))
                    .Resolve(OnSuccess: async (user) =>
                    {
                        await context.Response.Ok(user);
                    },
                    OnFailure: async (error, user) =>
                    {
                        switch (error)
                        {
                            case FindUserByUsernameCommandException.UserNotFound:
                                await context.Response.NotFound(new { Message = error });
                                break;
                        }
                    });
            }).RequireAuthorization();

            return endpoints;
        }
    }
}