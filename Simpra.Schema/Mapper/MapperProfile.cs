using AutoMapper;
using Simpra.Core.Entity;
using Simpra.Schema.CategoryRR;
using Simpra.Schema.CouponRR;
using Simpra.Schema.OrderRR;
using Simpra.Schema.ProductRR;
using Simpra.Schema.ProductwithCategoryRR;


namespace Simpra.Schema.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, CategorywithProductResponse>().ReverseMap();

            CreateMap<Category, CategoryResponse>().ReverseMap();
            CreateMap<Category, CategoryCreateRequest>().ReverseMap();
            CreateMap<CategoryUpdateRequest, Category>().ReverseMap();

            CreateMap<Product, ProductResponse>();
            CreateMap<ProductUpdateRequest, Product>().ReverseMap();
            CreateMap<ProductCreateRequest, Product>().ReverseMap();

            CreateMap<Order, OrderResponse>().ReverseMap();
            CreateMap<OrderUpdateRequest, Order>().ReverseMap();
            CreateMap<OrderCreateRequest, Order>().ReverseMap();
            CreateMap<OrderDetailRequest, OrderDetail>().ReverseMap();
            CreateMap<OrderDetailResponse, OrderDetail>().ReverseMap();

            CreateMap<Coupon, CouponResponse>().ReverseMap();
            CreateMap<CouponUpdateRequest, Coupon>().ReverseMap();
            CreateMap<CouponCreateRequest, Coupon>().ReverseMap();
            
        }
    }
}
