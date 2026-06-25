using BackendProject.Entities;

namespace BackendProject.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
}