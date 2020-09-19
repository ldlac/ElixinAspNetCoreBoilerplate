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
using System.Threading.Tasks;

namespace ElixinBackend.Users
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddUsers(this IServiceCollection serviceCollection, AppSettings appSettings)
        {
            return serviceCollection
                .AddMediatR(typeof(ConfigureServices).GetTypeInfo().Assembly)
                .AddTransient<IAuthentificationService, AuthentificationService>()
                .AddAuthorization()
                .ConfigureAuthentication(appSettings);
        }

        private static IServiceCollection ConfigureAuthentication(this IServiceCollection serviceCollection, AppSettings appSettings)
        {
            serviceCollection
                .AddAuthentication(ConfigureAuthenticationOptions)
                .AddJwtBearer(x => ConfigureJwtBearerOptions(x, appSettings));
            return serviceCollection;
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
                    if (usernameClaim is null)
                    {
                        context.Fail("Unauthorized");
                    }

                    var command = new FindUserByUsernameCommand() { Username = usernameClaim };

                    await (await mediator.Send(command))
                        .Resolve(OnFailure: async (error, user) =>
                        {
                            context.Fail("Unauthorized");
                            await Task.CompletedTask;
                        });
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