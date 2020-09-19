using ElixinBackend.Users.Services;
using ElixinBackend.Utils;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using static ElixinBackend.Users.Services.IAuthentificationService;

namespace ElixinBackend.Users.UseCases.AuthenticateUseCase
{
    public static class AuthenticateEndpoint
    {
        public static IEndpointRouteBuilder MapAuthenticate(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost<AuthenticateUser>("/authenticate", async (serviceProvider, authenticateUser, response) =>
            {
                var authService = serviceProvider.GetRequiredService<IAuthentificationService>();

                await (await authService.Authenticate(authenticateUser.Username, authenticateUser.Password))
                    .Resolve(OnSuccess: async user =>
                    {
                        var tokenString = authService.GetJwtToken(user);

                        await response.Ok(new
                        {
                            Token = tokenString
                        });
                    },
                    OnFailure: async (error, user) =>
                    {
                        switch (error)
                        {
                            case AuthentificationServiceException.AuthentificationFailed:
                                await response.BadRequest(new { Message = error });
                                break;
                        }
                    });
            });

            return endpoints;
        }
    }
}