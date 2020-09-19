using ElixinBackend.Users;
using Microsoft.EntityFrameworkCore;

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
        public ElixinBackendContext()
        {
        }

        public ElixinBackendContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=ElixinBackend;User Id=postgres;Password=postgres;");
        }
    }
}