using Microsoft.EntityFrameworkCore;

namespace PortfolioServer.Api.Entity
{
    public class PortfolioDbContext : DbContext
    {
        public DbSet<Portfolio> Portfolios => Set<Portfolio>();
        public DbSet<Asset> Assets => Set<Asset>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options)
            : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new PortfolioEntityTypeConfiguration().Configure(modelBuilder.Entity<Portfolio>());
            new AssetEntityTypeConfiguration().Configure(modelBuilder.Entity<Asset>());
            new TransactionEntityTypeConfiguration().Configure(modelBuilder.Entity<Transaction>());
        }
    }
}