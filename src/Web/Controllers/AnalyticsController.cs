using MediatR;
using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.Application.Analytics.Queries.GetSalesMetrics;

namespace CleanArchitecture.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnalyticsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("sales-metrics")]
    public async Task<IActionResult> GetSalesMetrics()
    {
        var result = await _mediator.Send(new GetSalesMetricsQuery());
        return Ok(result);
    }
} 