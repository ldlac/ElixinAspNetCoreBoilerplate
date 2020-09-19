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
                var username = (context.Request.RouteValues["username"]).ToString();

                var mediator = context.RequestServices.GetRequiredService<IMediator>();

                var command = new GetUserByCommand() { Username = username };

                try
                {
                    var user = await mediator.Send(command);

                    await context.Response.Ok(user);
                }
                catch (UserNotFoundException ex)
                {
                    await context.Response.NotFound(new { ex.Message });
                }
            });

            return endpoints;
        }
    }
}