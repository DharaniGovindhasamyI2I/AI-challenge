using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.Web.Services;
using CleanArchitecture.Application.Orders.Common;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SignalRTestController : ControllerBase
{
    private readonly ISignalRNotificationService _notificationService;

    public SignalRTestController(ISignalRNotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost("order-created")]
    public async Task<IActionResult> TestOrderCreated()
    {
        var testOrder = new OrderDto
        {
            Id = 1,
            OrderNumber = "ORD-20241201-TEST001",
            CustomerId = 1,
            Status = OrderStatus.Pending,
            PaymentStatus = PaymentStatus.Pending,
            TotalAmount = 299.99m,
            ShippingAddress = new AddressDto
            {
                Street = "123 Test St",
                City = "Test City",
                State = "TS",
                ZipCode = "12345",
                Country = "Test Country"
            },
            Items = new List<OrderItemDto>
            {
                new() { Id = 1, ProductId = 1, ProductName = "Test Product", UnitPrice = 149.99m, Quantity = 2, TotalAmount = 299.98m }
            },
            Created = DateTime.UtcNow
        };

        await _notificationService.NotifyOrderCreatedAsync(testOrder);
        return Ok("Order created notification sent");
    }

    [HttpPost("order-status-changed")]
    public async Task<IActionResult> TestOrderStatusChanged()
    {
        var testOrder = new OrderDto
        {
            Id = 1,
            OrderNumber = "ORD-20241201-TEST001",
            CustomerId = 1,
            Status = OrderStatus.Confirmed,
            PaymentStatus = PaymentStatus.Paid,
            TotalAmount = 299.99m
        };

        await _notificationService.NotifyOrderStatusChangedAsync(testOrder, "Pending");
        return Ok("Order status changed notification sent");
    }

    [HttpPost("payment-processed")]
    public async Task<IActionResult> TestPaymentProcessed()
    {
        var testOrder = new OrderDto
        {
            Id = 1,
            OrderNumber = "ORD-20241201-TEST001",
            CustomerId = 1,
            TotalAmount = 299.99m
        };

        await _notificationService.NotifyPaymentProcessedAsync(testOrder, "Paid");
        return Ok("Payment processed notification sent");
    }

    [HttpPost("inventory-updated")]
    public async Task<IActionResult> TestInventoryUpdated([FromQuery] int productId = 1, [FromQuery] int newQuantity = 50, [FromQuery] int oldQuantity = 100)
    {
        await _notificationService.NotifyInventoryUpdatedAsync(productId, newQuantity, oldQuantity);
        return Ok($"Inventory updated notification sent for product {productId}");
    }

    [HttpPost("low-stock-alert")]
    public async Task<IActionResult> TestLowStockAlert([FromQuery] int productId = 1, [FromQuery] string productName = "Test Product", [FromQuery] int quantity = 5)
    {
        await _notificationService.NotifyLowStockAlertAsync(productId, productName, quantity);
        return Ok($"Low stock alert sent for product {productName}");
    }
} 