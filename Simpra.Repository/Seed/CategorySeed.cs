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
                    Name = "E-book",
                    Url = "www.test1.com",
                    Tag = "test1",
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Sezin"
                },
                new Category
                {
                    Id = 2,
                    Name = "Videos",
                    Url = "www.test1.com",
                    Tag = "test1",
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Sezin"
                },
                new Category
                {
                    Id = 3,
                    Name = "Animation",
                    Url = "www.test1.com",
                    Tag = "test1",
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Sezin"
                },
                new Category
                {
                    Id = 4,
                    Name = "stok fotoğraflar",
                    Url = "www.test1.com",
                    Tag = "test1",
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Sezin"
                }
                );
        }
    }
}
