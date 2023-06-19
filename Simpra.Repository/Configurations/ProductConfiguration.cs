using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpra.Core.Entity;

namespace Simpra.Repository.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.CreatedAt).IsRequired(true);
            builder.Property(x => x.CreatedBy).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.UpdatedAt).IsRequired(false);
            builder.Property(x => x.UpdatedBy).IsRequired(false).HasMaxLength(30);

            builder.Property(x => x.Name).IsRequired(true).HasMaxLength(100);
            builder.Property(x => x.Stock).IsRequired(true);
            builder.Property(x => x.Price).IsRequired(true).HasColumnType("decimal(18,2)");

            builder.Property(x => x.Property).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.Definition).IsRequired(false).HasMaxLength(100);

            builder.Property(x => x.IsActive).IsRequired(true);
            builder.Property(x => x.EarningPercentage).IsRequired(true);
            builder.Property(x => x.MaxPuanAmount).IsRequired(true);

            builder.HasIndex(x => x.Name).IsUnique(true);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .IsRequired(true);
        }
    }
}
