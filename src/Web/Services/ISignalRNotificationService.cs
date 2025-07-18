using CleanArchitecture.Application.Orders.Common;

namespace CleanArchitecture.Web.Services;

public interface ISignalRNotificationService
{
    Task NotifyOrderCreatedAsync(OrderDto order);
    Task NotifyOrderStatusChangedAsync(OrderDto order, string previousStatus);
    Task NotifyPaymentProcessedAsync(OrderDto order, string paymentStatus);
    Task NotifyInventoryUpdatedAsync(int productId, int newQuantity, int oldQuantity);
    Task NotifyLowStockAlertAsync(int productId, string productName, int currentQuantity);
    Task NotifyNewOrderToAdminsAsync(OrderDto order);
    Task NotifyOrderToCustomerAsync(OrderDto order, int customerId);
} 