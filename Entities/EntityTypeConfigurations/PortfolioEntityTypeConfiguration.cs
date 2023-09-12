using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PortfolioServer.Api.Entity
{
    public class PortfolioEntityTypeConfiguration : IEntityTypeConfiguration<Portfolio>
    {
        public void Configure(EntityTypeBuilder<Portfolio> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name).HasMaxLength(256);

            builder.HasMany(b => b.Assets)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Portfolios");
        }
    }
}