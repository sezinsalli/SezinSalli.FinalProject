using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpra.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Repository.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            
            builder.Property(x=>x.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.BillingAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.CouponAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.WalletAmount).IsRequired().HasColumnType("decimal(18,2)");

            builder.Property(x => x.IsActive).IsRequired();
            builder.HasOne(x => x.User).WithMany(x => x.Orders).HasForeignKey(x => x.UserId);
        }
    }
}
