using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Products.Common;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Data
{
    public class ProductCacheService : IProductCacheService
    {
        private readonly IDistributedCache _cache;
        private const string ProductKeyPrefix = "product:";
        private const string AllProductsKey = "products:all";
        private static readonly DistributedCacheEntryOptions CacheOptions = new() { AbsoluteExpirationRelativeToNow = System.TimeSpan.FromMinutes(10) };

        public ProductCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<ProductDto?> GetProductAsync(int id)
        {
            var data = await _cache.GetStringAsync(ProductKeyPrefix + id);
            return data is null ? null : JsonSerializer.Deserialize<ProductDto>(data);
        }

        public async Task SetProductAsync(ProductDto product)
        {
            var data = JsonSerializer.Serialize(product);
            await _cache.SetStringAsync(ProductKeyPrefix + product.Id, data, CacheOptions);
        }

        public async Task RemoveProductAsync(int id)
        {
            await _cache.RemoveAsync(ProductKeyPrefix + id);
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            var data = await _cache.GetStringAsync(AllProductsKey);
            return data is null ? new List<ProductDto>() : JsonSerializer.Deserialize<List<ProductDto>>(data) ?? new List<ProductDto>();
        }

        public async Task SetAllProductsAsync(List<ProductDto> products)
        {
            var data = JsonSerializer.Serialize(products);
            await _cache.SetStringAsync(AllProductsKey, data, CacheOptions);
        }
    }
} 