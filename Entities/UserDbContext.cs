using Microsoft.EntityFrameworkCore;

namespace PortfolioServer.Api.Entity
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<LoginProvider> UserLogins => Set<LoginProvider>();

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
            new LoginProviderEntityTypeConfiguration().Configure(modelBuilder.Entity<LoginProvider>());
        }
    }
}