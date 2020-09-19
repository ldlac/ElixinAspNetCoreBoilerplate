using ElixinBackend.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElixinBackend.DAL
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddDbContext(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            return serviceCollection
                .AddDbContext<ElixinBackendContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")))
                .AddTransient<IUserDbContext, ElixinBackendContext>();
        }
    }
}