using MediatR;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Analytics.Queries.GetSalesMetrics;

public class GetSalesMetricsQueryHandler : IRequestHandler<GetSalesMetricsQuery, SalesMetricsDto>
{
    private readonly IApplicationDbContext _context;

    public GetSalesMetricsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SalesMetricsDto> Handle(GetSalesMetricsQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        // TODO: Add analytics logic
        return new SalesMetricsDto();
    }
} 