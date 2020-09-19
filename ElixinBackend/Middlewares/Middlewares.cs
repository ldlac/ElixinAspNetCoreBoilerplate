using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ElixinBackend.Utils;
using Microsoft.AspNetCore.Diagnostics;

namespace ElixinBackend.Middlewares
{
    public static class Middlewares
    {
        public static IApplicationBuilder UseLogInvalidEndpointsMiddleware(this IApplicationBuilder app)
        {
            app.Use(next => context =>
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<Startup>>();

                logger.LogWarning($"Endpoint: {context.Request.Path.Value} doesn't exists");

                return next(context);
            });

            return app;
        }

        public static void UseCustomErrors(this IApplicationBuilder app, IHostEnvironment environment)
        {
            app.Use(next => context =>
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<Startup>>();

                var exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();
                var ex = exceptionDetails?.Error;

                if (ex is null)
                {
                    logger.LogError(ex, "Unknown Error Occurred");
                }
                else
                {
                    logger.LogError(ex, ex.Message);
                }

                context.Response.WriteJson(new { Message = "An unexpected error occurred!" });

                return next(context);
            });
        }
    }
}