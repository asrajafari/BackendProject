using BackendProject.Entities;
using BackendProject.Repositories;

namespace BackendProject.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _repo.GetAllAsync();
    }
}