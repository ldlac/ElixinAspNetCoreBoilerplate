using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace ElixinBackend.Middlewares
{
    public static class Middlewares
    {
        public static IApplicationBuilder UseLogInvalidEndpointsMiddleware(this IApplicationBuilder app)
        {
            app.Use(next => context =>
            {
                var logger = context.RequestServices.GetService<ILogger<Startup>>();

                logger.LogWarning($"Endpoint: {context.Request.Path.Value} doesn't exists");

                return next(context);
            });

            return app;
        }
    }
}