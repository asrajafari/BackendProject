using AutoMapper;
using BackendProject.Application.DTOs.Carts.Responses;
using BackendProject.Application.DTOs.Products.Requests;
using BackendProject.Application.DTOs.Products.Responses;
using BackendProject.Application.DTOs.Wallet.Responses;
using BackendProject.Domain.Entities;

namespace BackendProject.Application.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, GetProductResponseDto>();

        CreateMap<Product, GetProductsResponseDto>();

        CreateMap<Product, CreateProductResponseDto>();

        CreateMap<Product, UpdateProductResponseDto>();

        CreateMap<Product, DeleteProductResponseDto>();
        
        CreateMap<Wallet, WalletResponseDto>();

        CreateMap<WalletTransaction, WalletTransactionResponseDto>();

        CreateMap<CartItem, CartItemResponseDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price));
    }
}