using AutoMapper;
using BackendProject.Application.DTOs.Products.Requests;
using BackendProject.Application.DTOs.Products.Responses;
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
    }
}