using ElixinBackend.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ElixinBackend.DAL
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddDbContext(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddDbContext<ElixinBackendContext>(options =>
                    options.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ElixinBackend;User Id=postgres;Password=postgres;"))

                .AddTransient<IUserDbContext, ElixinBackendContext>();

            return serviceCollection;
        }
    }
}