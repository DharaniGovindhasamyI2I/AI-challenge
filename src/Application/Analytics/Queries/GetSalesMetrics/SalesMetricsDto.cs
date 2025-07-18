namespace CleanArchitecture.Application.Analytics.Queries.GetSalesMetrics;

public class SalesMetricsDto
{
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TotalCustomers { get; set; }
    public int TotalProductsSold { get; set; }
    public List<TopProductDto> TopProducts { get; set; } = new();
}

public class TopProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public decimal Revenue { get; set; }
} 