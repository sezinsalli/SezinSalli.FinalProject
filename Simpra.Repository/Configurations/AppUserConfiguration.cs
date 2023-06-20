using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpra.Core.Entity;

namespace Simpra.Repository.Configurations
{
    internal class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.FirstName).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.LastName).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.DigitalWalletBalance).IsRequired(true).HasColumnType("decimal(18,2)");
            builder.Property(x => x.DigitalWalletInformation).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.CreatedAt).IsRequired(true);
            builder.Property(x => x.CreatedBy).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.UpdatedAt).IsRequired(false);
            builder.Property(x => x.UpdatedBy).IsRequired(false).HasMaxLength(30);
            builder.HasIndex(x => x.UserName).IsUnique(true);
        }
    }
}
