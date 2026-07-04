using AutoMapper;
using BackendProject.Application.DTOs.Products.Requests;
using BackendProject.Application.DTOs.Products.Responses;
using BackendProject.Application.Interfaces;
using BackendProject.Domain.Entities;

namespace BackendProject.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(
        IProductRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetProductsResponseDto>> GetAllAsync(GetProductsRequestDto requestDto)
    {
        var products = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<GetProductsResponseDto>>(products);
    }

    public async Task<GetProductResponseDto?> GetByIdAsync(GetProductRequestDto requestDto)
    {
        var product = await _repository.GetByIdAsync(requestDto.Id);

        if (product is null)
            return null;

        return _mapper.Map<GetProductResponseDto>(product);
    }

    public async Task<CreateProductResponseDto> CreateAsync(CreateProductRequestDto requestDto)
    {
        var product = new Product(
            requestDto.Name,
            requestDto.Price);

        _repository.Add(product);

        await _repository.SaveChangesAsync();

        return _mapper.Map<CreateProductResponseDto>(product);
    }

    public async Task<UpdateProductResponseDto> UpdateAsync(UpdateProductRequestDto requestDto)
    {
        var product = await _repository.GetByIdAsync(requestDto.Id);

        if (product is null)
            throw new Exception("Product not found.");

        product.ChangeName(requestDto.Name);
        product.ChangePrice(requestDto.Price);

        _repository.Update(product);

        await _repository.SaveChangesAsync();

        return _mapper.Map<UpdateProductResponseDto>(product);
    }

    public async Task<DeleteProductResponseDto> DeleteAsync(DeleteProductRequestDto requestDto)
    {
        var product = await _repository.GetByIdAsync(requestDto.Id);

        if (product is null)
            throw new Exception("Product not found.");

        _repository.Delete(product);

        await _repository.SaveChangesAsync();

        return new DeleteProductResponseDto
        {
            IsSuccess = true,
            Message = "Product deleted successfully."
        };
    }
}