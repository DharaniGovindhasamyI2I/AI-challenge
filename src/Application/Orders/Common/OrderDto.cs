using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Orders.Common;

public class OrderDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public decimal TotalAmount { get; set; }
    public AddressDto ShippingAddress { get; set; } = default!;
    public string? Notes { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}

public class AddressDto
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}

public class OrderItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalAmount { get; set; }
} 