using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simpra.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Repository.Seed
{
    internal class CategorySeed : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category { Id = 1, Name = "E-book",url="www.test1.com",tag="test1" },
                new Category { Id = 2, Name = "Videos", url = "www.test1.com", tag = "test1" },
                new Category { Id = 3, Name = "Animation", url = "www.test1.com", tag = "test1" },
                new Category { Id = 4, Name = "stok fotoğraflar", url = "www.test1.com", tag = "test1" }
                );
        }
    }
}
