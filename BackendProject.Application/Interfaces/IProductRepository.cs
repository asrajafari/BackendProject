using BackendProject.Domain.Entities;

namespace BackendProject.Application.Interfaces;

public interface IProductRepository : IGenericRepository<Product, int>
{
}