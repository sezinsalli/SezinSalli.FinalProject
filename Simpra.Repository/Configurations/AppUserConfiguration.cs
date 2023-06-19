using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpra.Core.Entity;

namespace Simpra.Repository.Configurations
{
    internal class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.DigitalWalletInformation).IsRequired(true);
            builder.Property(x => x.DigitalWalletBalance).IsRequired(true).HasColumnType("decimal(18,2)");

            builder.HasIndex(x => x.UserName).IsUnique();
        }
    }
}
