using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PortfolioServer.Api.Entity
{
    public class LoginProviderEntityTypeConfiguration : IEntityTypeConfiguration<LoginProvider>
    {
        public void Configure(EntityTypeBuilder<LoginProvider> builder)
        {
            builder.HasKey(b => new { b.Provider, b.Key });

            builder.Property(b => b.Provider).HasMaxLength(256);
            builder.Property(b => b.Key).HasMaxLength(256);

            builder.ToTable("UserLogins");
        }
    }
}