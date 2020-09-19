using ElixinBackend.Users.Services;
using ElixinBackend.Utils;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace ElixinBackend.Users.UseCases.AuthenticateUseCase
{
    public static class AuthenticateEndpoint
    {
        public static IEndpointRouteBuilder MapAuthenticate(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost<AuthenticateUser>("/authenticate", async (serviceProvider, authenticateUser, response) =>
            {
                try
                {
                    var authService = serviceProvider.GetRequiredService<IAuthentificationService>();

                    var user = await authService.Authenticate(authenticateUser.Username, authenticateUser.Password);

                    var tokenString = authService.GetJwtToken(user);

                    await response.Ok(new
                    {
                        Token = tokenString
                    });
                }
                catch (AuthentificationFailedException ex)
                {
                    await response.BadRequest(new { ex.Message });
                }
            });

            return endpoints;
        }
    }
}