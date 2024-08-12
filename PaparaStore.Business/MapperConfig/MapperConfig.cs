using AutoMapper;
using Azure.Core;
using PaparaStore.Data.Domain;
using PaparaStore.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaparaStore.Business.MapperConfig;
public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<User, UserResponse>();
        CreateMap<UserRequest, User>();
        

        CreateMap<UserContact, UserContactResponse>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName));
        CreateMap<UserContactRequest, UserContact>();

        CreateMap<Wallet , WalletResponse>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName));
        CreateMap<WalletRequest, Wallet>();

        CreateMap<WalletTransaction, WalletTransactionResponse>();
        CreateMap<WalletTransactionRequest, WalletTransaction>();

        CreateMap<Category, CategoryResponse>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags != null ? src.Tags.ToList() : new List<string>()));
        CreateMap<CategoryRequest, Category>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags ?? new List<string>()));


        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.CategoryNames, opt => opt.MapFrom(src => src.CategoryProducts != null ? src.CategoryProducts.Where(cp => cp.Category != null).Select(cp => cp.Category.Name).ToList() : new List<string>()))
            .ForMember(dest => dest.CategoryTags, opt => opt.MapFrom(src => src.CategoryProducts != null? src.CategoryProducts.Where(cp => cp.Category != null).SelectMany(cp => cp.Category.Tags).ToList() : new List<string>()))
            .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.CategoryProducts != null ? src.CategoryProducts.Where(cp => cp.Category != null).Select(cp => cp.CategoryId).ToList() : new List<long>()));

        CreateMap<ProductRequest, Product>()
            .ForMember(dest => dest.CategoryProducts, opt => opt.MapFrom(src => src.CategoryIds.Select(id => new CategoryProduct { CategoryId = id })))
            .ForMember(dest => dest.ProductCodes, opt => opt.Ignore());//MapFrom(src => src.ProductCodes.Select(code => new ProductCode { Code = code }))); ;

        CreateMap<Cart, CartResponse>();
        CreateMap<CartRequest, Cart>();

        CreateMap<CartProduct, CartProductResponse>();
        CreateMap<CartProductRequest, CartProduct>();

        CreateMap<Category, CategoryResponse>();
        CreateMap<CategoryRequest, Category>();

        CreateMap<CategoryProduct, CategoryProductResponse>();
        CreateMap<CategoryProductRequest, CategoryProduct>();

        CreateMap<Coupon, CouponResponse>();
        CreateMap<CouponRequest, Coupon>();

        CreateMap<Order, OrderResponse>();
        CreateMap<OrderRequest, Order>();

        CreateMap<OrderDetail, OrderDetailResponse>()
             .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.Order.OrderNumber))
             .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
        CreateMap<OrderDetailRequest, OrderDetail>();

    }
}
