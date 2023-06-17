using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpra.Core.Entity;

namespace Simpra.Repository.Seed
{
    internal class ProductSeed : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                new Product
                {
                    Id = 1,
                    CategoryId = 1,
                    Name = "Product 1",
                    Stock = 10,
                    Price = 9.99m,
                    Property = "Property 1",
                    Definition = "Definition 1",
                    IsActive = true,
                    EarningPercentage = 0.5,
                    MaxPuanAmount = 100.0,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Sezin"
                }
            );
        }
    }




}
