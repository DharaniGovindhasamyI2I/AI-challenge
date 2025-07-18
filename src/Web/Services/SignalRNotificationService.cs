using Microsoft.AspNetCore.SignalR;
using CleanArchitecture.Application.Orders.Common;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Web.Services;

public class SignalRNotificationService : ISignalRNotificationService
{
    private readonly IHubContext<OrderHub> _orderHub;
    private readonly IHubContext<InventoryHub> _inventoryHub;
    private readonly ILogger<SignalRNotificationService> _logger;

    public SignalRNotificationService(
        IHubContext<OrderHub> orderHub,
        IHubContext<InventoryHub> inventoryHub,
        ILogger<SignalRNotificationService> logger)
    {
        _orderHub = orderHub;
        _inventoryHub = inventoryHub;
        _logger = logger;
    }

    public async Task NotifyOrderCreatedAsync(OrderDto order)
    {
        try
        {
            // Notify admins about new order
            await _orderHub.Clients.Group("admins").SendAsync("OrderCreated", order);
            
            // Notify customer about their new order
            await _orderHub.Clients.Group($"customer_{order.CustomerId}").SendAsync("OrderCreated", order);
            
            _logger.LogInformation("Order created notification sent for order {OrderNumber}", order.OrderNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending order created notification for order {OrderNumber}", order.OrderNumber);
        }
    }

    public async Task NotifyOrderStatusChangedAsync(OrderDto order, string previousStatus)
    {
        try
        {
            var statusChange = new
            {
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                PreviousStatus = previousStatus,
                NewStatus = order.Status.ToString(),
                ChangedAt = DateTime.UtcNow
            };

            // Notify order-specific group
            await _orderHub.Clients.Group($"order_{order.Id}").SendAsync("OrderStatusChanged", statusChange);
            
            // Notify customer
            await _orderHub.Clients.Group($"customer_{order.CustomerId}").SendAsync("OrderStatusChanged", statusChange);
            
            // Notify admins
            await _orderHub.Clients.Group("admins").SendAsync("OrderStatusChanged", statusChange);
            
            _logger.LogInformation("Order status changed notification sent for order {OrderNumber}: {PreviousStatus} -> {NewStatus}", 
                order.OrderNumber, previousStatus, order.Status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending order status changed notification for order {OrderNumber}", order.OrderNumber);
        }
    }

    public async Task NotifyPaymentProcessedAsync(OrderDto order, string paymentStatus)
    {
        try
        {
            var paymentNotification = new
            {
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                PaymentStatus = paymentStatus,
                Amount = order.TotalAmount,
                ProcessedAt = DateTime.UtcNow
            };

            // Notify order-specific group
            await _orderHub.Clients.Group($"order_{order.Id}").SendAsync("PaymentProcessed", paymentNotification);
            
            // Notify customer
            await _orderHub.Clients.Group($"customer_{order.CustomerId}").SendAsync("PaymentProcessed", paymentNotification);
            
            // Notify admins
            await _orderHub.Clients.Group("admins").SendAsync("PaymentProcessed", paymentNotification);
            
            _logger.LogInformation("Payment processed notification sent for order {OrderNumber}: {PaymentStatus}", 
                order.OrderNumber, paymentStatus);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending payment processed notification for order {OrderNumber}", order.OrderNumber);
        }
    }

    public async Task NotifyInventoryUpdatedAsync(int productId, int newQuantity, int oldQuantity)
    {
        try
        {
            var inventoryUpdate = new
            {
                ProductId = productId,
                OldQuantity = oldQuantity,
                NewQuantity = newQuantity,
                Change = newQuantity - oldQuantity,
                UpdatedAt = DateTime.UtcNow
            };

            // Notify product-specific group
            await _inventoryHub.Clients.Group($"product_{productId}").SendAsync("InventoryUpdated", inventoryUpdate);
            
            // Notify general inventory group
            await _inventoryHub.Clients.Group("inventory_updates").SendAsync("InventoryUpdated", inventoryUpdate);
            
            _logger.LogInformation("Inventory updated notification sent for product {ProductId}: {OldQuantity} -> {NewQuantity}", 
                productId, oldQuantity, newQuantity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending inventory updated notification for product {ProductId}", productId);
        }
    }

    public async Task NotifyLowStockAlertAsync(int productId, string productName, int currentQuantity)
    {
        try
        {
            var lowStockAlert = new
            {
                ProductId = productId,
                ProductName = productName,
                CurrentQuantity = currentQuantity,
                AlertedAt = DateTime.UtcNow
            };

            // Notify low stock alerts group
            await _inventoryHub.Clients.Group("low_stock_alerts").SendAsync("LowStockAlert", lowStockAlert);
            
            // Notify admins
            await _orderHub.Clients.Group("admins").SendAsync("LowStockAlert", lowStockAlert);
            
            _logger.LogWarning("Low stock alert sent for product {ProductName} (ID: {ProductId}): {CurrentQuantity} remaining", 
                productName, productId, currentQuantity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending low stock alert for product {ProductId}", productId);
        }
    }

    public async Task NotifyNewOrderToAdminsAsync(OrderDto order)
    {
        try
        {
            await _orderHub.Clients.Group("admins").SendAsync("NewOrderReceived", order);
            _logger.LogInformation("New order notification sent to admins for order {OrderNumber}", order.OrderNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending new order notification to admins for order {OrderNumber}", order.OrderNumber);
        }
    }

    public async Task NotifyOrderToCustomerAsync(OrderDto order, int customerId)
    {
        try
        {
            await _orderHub.Clients.Group($"customer_{customerId}").SendAsync("OrderUpdate", order);
            _logger.LogInformation("Order update notification sent to customer {CustomerId} for order {OrderNumber}", 
                customerId, order.OrderNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending order update notification to customer {CustomerId} for order {OrderNumber}", 
                customerId, order.OrderNumber);
        }
    }
} 