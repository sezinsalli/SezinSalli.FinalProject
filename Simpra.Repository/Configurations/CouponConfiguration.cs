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
    internal class CouponConfiguration:IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CouponCode).HasMaxLength(10);
            builder.Property(x => x.DiscountAmount).IsRequired().HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.User).WithOne(x => x.Coupon).HasForeignKey<Coupon>(x => x.UserId);
        }
    }
}
