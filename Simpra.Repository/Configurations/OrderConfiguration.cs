using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpra.Core.Entity;

namespace Simpra.Repository.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.CreatedAt).IsRequired(true);
            builder.Property(x => x.CreatedBy).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.UpdatedAt).IsRequired(false);
            builder.Property(x => x.UpdatedBy).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.OrderNumber).IsRequired(true).HasMaxLength(9);
            builder.Property(x => x.Status).IsRequired(true);
            builder.Property(x => x.IsActive).IsRequired(true);
            builder.Property(x => x.TotalAmount).IsRequired(true).HasColumnType("decimal(18,2)");
            builder.Property(x => x.BillingAmount).IsRequired(true).HasColumnType("decimal(18,2)");
            builder.Property(x => x.CouponAmount).IsRequired(true).HasColumnType("decimal(18,2)");
            builder.Property(x => x.WalletAmount).IsRequired(true).HasColumnType("decimal(18,2)");
            builder.Property(x => x.CouponCode).IsRequired(false).HasMaxLength(10);
            builder.HasIndex(x => x.OrderNumber).IsUnique(true);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId)
                .IsRequired(true);
        }
    }
}
