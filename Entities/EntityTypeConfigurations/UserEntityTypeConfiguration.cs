using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PortfolioServer.Api.Entity
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.GivenName).HasMaxLength(256);
            builder.Property(b => b.Surname).HasMaxLength(256);
            builder.Property(b => b.Username).HasMaxLength(256);
            builder.Property(b => b.Email).HasMaxLength(256);

            builder.HasMany<LoginProvider>()
                   .WithOne()
                   .HasForeignKey(b => b.UserId)
                   .IsRequired();

            builder.ToTable("Users");
        }
    }
}