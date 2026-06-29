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

    public async Task<IEnumerable<GetProductsResponse>> GetAllAsync(GetProductsRequest request)
    {
        var products = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<GetProductsResponse>>(products);
    }

    public async Task<GetProductResponse?> GetByIdAsync(GetProductRequest request)
    {
        var product = await _repository.GetByIdAsync(request.Id);

        if (product is null)
            return null;

        return _mapper.Map<GetProductResponse>(product);
    }

    public async Task<CreateProductResponse> CreateAsync(CreateProductRequest request)
    {
        var product = new Product(
            request.Name,
            request.Price);

        _repository.Add(product);

        await _repository.SaveChangesAsync();

        return _mapper.Map<CreateProductResponse>(product);
    }

    public async Task<UpdateProductResponse> UpdateAsync(UpdateProductRequest request)
    {
        var product = await _repository.GetByIdAsync(request.Id);

        if (product is null)
            throw new Exception("Product not found.");

        product.ChangeName(request.Name);
        product.ChangePrice(request.Price);

        _repository.Update(product);

        await _repository.SaveChangesAsync();

        return _mapper.Map<UpdateProductResponse>(product);
    }

    public async Task<DeleteProductResponse> DeleteAsync(DeleteProductRequest request)
    {
        var product = await _repository.GetByIdAsync(request.Id);

        if (product is null)
            throw new Exception("Product not found.");

        _repository.Delete(product);

        await _repository.SaveChangesAsync();

        return new DeleteProductResponse
        {
            IsSuccess = true,
            Message = "Product deleted successfully."
        };
    }
}