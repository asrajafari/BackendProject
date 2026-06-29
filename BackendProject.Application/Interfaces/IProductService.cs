using BackendProject.Application.DTOs.Products.Requests;
using BackendProject.Application.DTOs.Products.Responses;

namespace BackendProject.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<GetProductsResponse>> GetAllAsync(GetProductsRequest request);

    Task<GetProductResponse?> GetByIdAsync(GetProductRequest request);

    Task<CreateProductResponse> CreateAsync(CreateProductRequest request);

    Task<UpdateProductResponse> UpdateAsync(UpdateProductRequest request);

    Task<DeleteProductResponse> DeleteAsync(DeleteProductRequest request);
}