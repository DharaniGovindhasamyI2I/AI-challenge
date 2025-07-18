using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using CleanArchitecture.Application.Products.Queries.GetProductById;
using CleanArchitecture.Application.Products.Common;

namespace CleanArchitecture.Application.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(p => p.Name.Contains(request.Name));

        if (request.CategoryId.HasValue)
            query = query.Where(p => p.CategoryId == request.CategoryId);

        if (request.MinPrice.HasValue)
            query = query.Where(p => p.Price >= request.MinPrice.Value);

        if (request.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= request.MaxPrice.Value);

        return await query
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
} 