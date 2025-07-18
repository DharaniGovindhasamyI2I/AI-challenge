using MediatR;
using System.Collections.Generic;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Products.Queries.GetProductById;
using CleanArchitecture.Application.Products.Common;

namespace CleanArchitecture.Application.Products.Queries.GetProducts;

public class GetProductsQuery : IRequest<List<ProductDto>>, ICacheableQuery
{
    public string? Name { get; set; }
    public int? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

    public string CacheKey => $"products_{Name}_{CategoryId}_{MinPrice}_{MaxPrice}";
    public TimeSpan? CacheExpiration => TimeSpan.FromMinutes(10);
} 