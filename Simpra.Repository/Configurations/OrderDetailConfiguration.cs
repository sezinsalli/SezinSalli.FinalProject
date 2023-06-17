using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpra.Core.Entity;

namespace Simpra.Repository.Configurations
{
    internal class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(od => new { od.OrderId, od.ProductId });

            builder.HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .IsRequired(true);

            builder.HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .IsRequired(true);

            builder.Property(x => x.Quantity).IsRequired(true);
            builder.Property(x => x.UnitPrice).IsRequired(true).HasColumnType("decimal(18,2)");

        }
    }
}
