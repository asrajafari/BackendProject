using BackendProject.Application.Interfaces;
using BackendProject.Domain.Entities;
using BackendProject.Infrastructure.Data;

namespace BackendProject.Infrastructure.Repositories;

public class ProductRepository
    : GenericRepository<Product, int>, IProductRepository
{
    public ProductRepository(AppDbContext context)
        : base(context)
    {
    }
}