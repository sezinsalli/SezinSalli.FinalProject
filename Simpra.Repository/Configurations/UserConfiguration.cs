using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpra.Core.Entity;

namespace Simpra.Repository.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.CreatedAt).IsRequired(true);
            builder.Property(x => x.CreatedBy).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.UpdatedAt).IsRequired(false);
            builder.Property(x => x.UpdatedBy).IsRequired(false).HasMaxLength(30);

            builder.Property(x => x.UserName).IsRequired(true);
            builder.Property(x => x.FirstName).IsRequired(true);
            builder.Property(x => x.LastName).IsRequired(true);
            builder.Property(x => x.DigitalWalletInformation).IsRequired(true);
            builder.Property(x => x.DigitalWalletBalance).IsRequired(true).HasColumnType("decimal(18,2)");

            builder.HasIndex(x=> x.UserName).IsUnique();
        }
    }
}
