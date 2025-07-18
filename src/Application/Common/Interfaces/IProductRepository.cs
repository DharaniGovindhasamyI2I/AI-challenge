using CleanArchitecture.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);
        Task<List<Product>> GetAllAsync();
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
    }
} 