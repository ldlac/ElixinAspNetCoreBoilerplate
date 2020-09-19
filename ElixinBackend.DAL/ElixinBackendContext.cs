using ElixinBackend.Users;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

/*          Commands
 *
 *  dotnet ef migrations add <NAME>
 *  dotnet ef database update
 *
 *
*/

namespace ElixinBackend.DAL
{
    public class ElixinBackendContext : DbContext, IUserDbContext
    {
        private const string DEVELOPMENT_CONNECTION_STRING = "Server=127.0.0.1;Port=5432;Database=ElixinBackend;User Id=postgres;Password=postgres;";

        public ElixinBackendContext()
        {
        }

        public ElixinBackendContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.Options.FindExtension<NpgsqlOptionsExtension>() is null)
            {
                optionsBuilder.UseNpgsql(DEVELOPMENT_CONNECTION_STRING);
            }
        }
    }
}