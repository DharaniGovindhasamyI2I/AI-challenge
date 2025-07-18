using MediatR;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Products.Common;

namespace CleanArchitecture.Application.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IApplicationDbContext _context;

    public GetProductByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        // TODO: Add fetching and mapping logic
        return null!;
    }
} 