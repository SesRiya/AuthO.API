using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;

namespace AuthenticationServerEntityFramework
{
    public class AuthenticationServerDbContext : DbContext
    {
        private readonly AuthenticationConfig _configuration;

        public AuthenticationServerDbContext(AuthenticationConfig configuration)
        {
            _configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.ConnectionStrings);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
