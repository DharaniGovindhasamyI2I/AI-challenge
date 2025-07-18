using CleanArchitecture.Application.Products.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Interfaces
{
    public interface IProductCacheService
    {
        Task<ProductDto?> GetProductAsync(int id);
        Task SetProductAsync(ProductDto product);
        Task RemoveProductAsync(int id);
        Task<List<ProductDto>> GetAllProductsAsync();
        Task SetAllProductsAsync(List<ProductDto> products);
    }
} 