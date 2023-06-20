using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpra.Core.Entity;

namespace Simpra.Repository.Seed
{
    internal class CategorySeed : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category
                {
                    Id = 1,
                    Name = "Category 1",
                    Url = "www.test.com",
                    Tag = "test1",
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Sezin"
                },
                new Category
                {
                    Id = 2,
                    Name = "Category 2",
                    Url = "www.test.com",
                    Tag = "test2",
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Sezin"
                }
                );
        }
    }
}
