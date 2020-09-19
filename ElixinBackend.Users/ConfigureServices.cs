using ElixinBackend.Shared;
using ElixinBackend.Users.Services;
using ElixinBackend.Users.UseCases.GetUserUseCase;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace ElixinBackend.Users
{
    public static class ConfigureServices
    {
        public static void AddUsers(this IServiceCollection serviceCollection, AppSettings appSettings)
        {
            serviceCollection
                .AddMediatR(typeof(ConfigureServices).GetTypeInfo().Assembly)

                .AddTransient<IAuthentificationService, AuthentificationService>()

                .AddAuthorization()
                .AddAuthentication(ConfigureAuthenticationOptions)
                .AddJwtBearer(x => ConfigureJwtBearerOptions(x, appSettings));
        }

        private static void ConfigureAuthenticationOptions(AuthenticationOptions x)
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }

        private static void ConfigureJwtBearerOptions(JwtBearerOptions x, AppSettings appSettings)
        {
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            x.Events = new JwtBearerEvents
            {
                OnTokenValidated = async context =>
                {
                    var mediator = context.HttpContext.RequestServices.GetRequiredService<IMediator>();

                    var usernameClaim = context.Principal.Identity.Name;
                    if (usernameClaim == null)
                    {
                        context.Fail("Unauthorized");
                    }

                    try
                    {
                        var command = new GetUserByCommand() { Username = usernameClaim };

                        await mediator.Send(command);
                    }
                    catch (UserNotFoundException)
                    {
                        context.Fail("Unauthorized");
                    }
                }
            };
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        }
    }
}