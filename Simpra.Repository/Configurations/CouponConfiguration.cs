using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpra.Core.Entity;

namespace Simpra.Repository.Configurations
{
    internal class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.CreatedAt).IsRequired(true);
            builder.Property(x => x.CreatedBy).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.UpdatedAt).IsRequired(false);
            builder.Property(x => x.UpdatedBy).IsRequired(false).HasMaxLength(30);

            builder.Property(x => x.CouponCode).IsRequired(true).HasMaxLength(10);
            builder.Property(x => x.DiscountAmount).IsRequired(true).HasColumnType("decimal(18,2)");
            builder.Property(x => x.ExpirationDate).IsRequired(true);

            builder.HasIndex(x => x.CouponCode).IsUnique(true);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Coupon)
                .HasForeignKey(x => x.UserId)
                .IsRequired(true);
        }
    }
}
