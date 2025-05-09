using IDP.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IDP.Infra.Data
{
    public class ShopDbContext:DbContext
    {
        protected readonly IConfiguration Configuration;

        public ShopDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseSqlServer(Configuration.GetConnectionString("CommandDBConnection"));
        }
        public DbSet<User> tbl_Users { get; set; }
    }
}
