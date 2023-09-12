using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PortfolioServer.Api.Entity
{
    public class AssetEntityTypeConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name).HasMaxLength(256);
            builder.Property(b => b.Symbol).HasMaxLength(256);
            builder.Property(b => b.Currency).HasMaxLength(256);

            builder.HasMany(b => b.Transactions)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Assets");
        }
    }
}