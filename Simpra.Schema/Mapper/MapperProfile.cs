using AutoMapper;
using Simpra.Core.Entity;
using Simpra.Schema.CategoryRR;
using Simpra.Schema.ProductRR;
using Simpra.Schema.ProductwithCategoryRR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Simpra.Schema.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, CategoryResponse>();
            CreateMap<CategoryRequest, Category>();

            CreateMap<Product, ProductwithCategoryResponse>().ReverseMap();
            CreateMap<Category, ProductwithCategoryResponse>().ReverseMap();
            
            


            CreateMap<Product, ProductResponse>();
            CreateMap<ProductRequest, Product>();

        }
    }
}
