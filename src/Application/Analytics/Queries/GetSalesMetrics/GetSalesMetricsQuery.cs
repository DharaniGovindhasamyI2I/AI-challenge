using MediatR;

namespace CleanArchitecture.Application.Analytics.Queries.GetSalesMetrics;

public record GetSalesMetricsQuery : IRequest<SalesMetricsDto>; 