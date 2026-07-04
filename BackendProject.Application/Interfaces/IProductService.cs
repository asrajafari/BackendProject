using BackendProject.Application.DTOs.Products.Requests;
using BackendProject.Application.DTOs.Products.Responses;

namespace BackendProject.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<GetProductsResponseDto>> GetAllAsync(GetProductsRequestDto requestDto);

    Task<GetProductResponseDto?> GetByIdAsync(GetProductRequestDto requestDto);

    Task<CreateProductResponseDto> CreateAsync(CreateProductRequestDto requestDto);

    Task<UpdateProductResponseDto> UpdateAsync(UpdateProductRequestDto requestDto);

    Task<DeleteProductResponseDto> DeleteAsync(DeleteProductRequestDto requestDto);
}