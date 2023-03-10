using Microsoft.EntityFrameworkCore;
using Models;

namespace AuthenticationServerEntityFramework
{
    public class AuthenticationServerDbContext : DbContext
    {
        public DbSet<User>? Users { get; set; }
        public DbSet<Role>? Roles { get; set; }
        public DbSet<UserRole>? UserRoles { get; set; }
        public DbSet<RefreshToken>? RefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\ProjectModels;Database=AuthServerDb;Trusted_Connection=True");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
