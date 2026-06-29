using AutoMapper;
using BackendProject.Application.DTOs.Products.Requests;
using BackendProject.Application.DTOs.Products.Responses;
using BackendProject.Domain.Entities;

namespace BackendProject.Application.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, GetProductResponse>();

        CreateMap<Product, GetProductsResponse>();

        CreateMap<Product, CreateProductResponse>();

        CreateMap<Product, UpdateProductResponse>();

        CreateMap<Product, DeleteProductResponse>();
    }
}