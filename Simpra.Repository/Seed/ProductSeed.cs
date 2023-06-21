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
                    Stock = 20,
                    Price = 100.00m,
                    Property = "Property 1",
                    Definition = "Definition 1",
                    IsActive = true,
                    Status = Core.Enum.ProductStatus.InStock,
                    EarningPercentage = 0.12,
                    MaxPuanAmount = 10.0,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Sezin"
                },
                new Product
                {
                    Id = 2,
                    CategoryId = 1,
                    Name = "Product 2",
                    Stock = 15,
                    Price = 200.00m,
                    Property = "Property 2",
                    Definition = "Definition 2",
                    IsActive = true,
                    Status = Core.Enum.ProductStatus.InStock,
                    EarningPercentage = 0.20,
                    MaxPuanAmount = 30.0,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Sezin"
                },
                new Product
                {
                    Id = 3,
                    CategoryId = 2,
                    Name = "Product 3",
                    Stock = 8,
                    Price = 50.00m,
                    Property = "Property 3",
                    Definition = "Definition 3",
                    IsActive = true,
                    Status = Core.Enum.ProductStatus.InStock,
                    EarningPercentage = 0.20,
                    MaxPuanAmount = 8.0,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Sezin"
                },
                new Product
                {
                    Id = 4,
                    CategoryId = 2,
                    Name = "Product 4",
                    Stock = 20,
                    Price = 100.00m,
                    Property = "Property 4",
                    Definition = "Definition 4",
                    IsActive = false,
                    Status = Core.Enum.ProductStatus.Discontinued,
                    EarningPercentage = 0.05,
                    MaxPuanAmount = 4.0,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Sezin"
                }
            );
        }
    }




}
