using ElixinBackend.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ElixinBackend.DAL;
using ElixinBackend.Shared;
using Microsoft.Extensions.Logging;
using ElixinBackend.Middlewares;

namespace ElixinBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");

            services
                .Configure<AppSettings>(appSettingsSection)
                .AddDbContext(Configuration)
                .AddCors()
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder
                        .ClearProviders()
                        .AddConfiguration(Configuration.GetSection("Logging"))
                        .AddConsole();
                })
                .AddUsers(appSettingsSection.Get<AppSettings>());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app
                    .UseDeveloperExceptionPage()
                    .UseDatabaseErrorPage();
            }
            else
            {
                app
                    .UseExceptionHandler(err => err.UseCustomErrors(env))
                    .UseHsts();
            }

            app
                .UseHttpsRedirection()
                .UseRouting()
                .UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader())
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapUsers();
                })
                .UseLogInvalidEndpointsMiddleware();
        }
    }
}