using IDP.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IDP.Infra.Data
{
    public class ShopCommandDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public ShopCommandDbContext(IConfiguration configuration)
        {
            Configuration = configuration;

        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseSqlServer(Configuration.GetConnectionString("CommandDBConnection"));
        }
        public DbSet<User> Tbl_Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
