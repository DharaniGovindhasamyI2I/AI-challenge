using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Orders;

public class OrderService : IOrderService
{
    private readonly IApplicationDbContext _context;
    private readonly IPaymentService _paymentService;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IApplicationDbContext context,
        IPaymentService paymentService,
        ILogger<OrderService> logger)
    {
        _context = context;
        _paymentService = paymentService;
        _logger = logger;
    }

    public async Task ValidateOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
        // Validate order has items
        if (!order.OrderItems.Any())
            throw new ValidationException("Order must have at least one item.");

        // Validate total amount
        if (order.TotalAmount.Amount <= 0)
            throw new ValidationException("Order total must be greater than zero.");

        // Validate customer exists and is active
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == order.CustomerId, cancellationToken);
        
        if (customer == null)
            throw new ValidationException("Customer not found.");

        // Validate all products exist and have sufficient inventory
        foreach (var item in order.OrderItems)
        {
            var product = await _context.Products
                .Include(p => p.Inventory)
                .FirstOrDefaultAsync(p => p.Id == item.ProductId, cancellationToken);

            if (product == null)
                throw new ValidationException($"Product with ID {item.ProductId} not found.");

            if (product.Inventory < item.Quantity)
                throw new ValidationException($"Insufficient inventory for product {product.Name}. Available: {product.Inventory}, Requested: {item.Quantity}");

            if (product.Price != item.UnitPrice.Amount)
                throw new ValidationException($"Product price mismatch for {product.Name}. Expected: {product.Price}, Actual: {item.UnitPrice.Amount}");
        }

        _logger.LogInformation("Order {OrderNumber} validation passed", order.OrderNumber);
    }

    public Task<bool> CanProcessPaymentAsync(Order order, CancellationToken cancellationToken = default)
    {
        // Check if order is in a valid state for payment
        if (order.Status != OrderStatus.Pending && order.Status != OrderStatus.Confirmed)
            return Task.FromResult(false);

        // Check if payment is already processed
        if (order.PaymentStatus == PaymentStatus.Paid)
            return Task.FromResult(false);

        // Additional business rules can be added here
        // e.g., check customer credit limit, payment method validity, etc.

        return Task.FromResult(true);
    }

    public async Task ProcessPaymentAsync(Order order, CancellationToken cancellationToken = default)
    {
        if (!await CanProcessPaymentAsync(order, cancellationToken))
            throw new ValidationException("Cannot process payment for this order.");

        try
        {
            // Create payment record
            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = order.TotalAmount,
                Status = PaymentStatus.Pending
            };

            _context.Payments.Add(payment);

            // Process payment through payment service
            var result = await _paymentService.ProcessPaymentAsync(payment, cancellationToken);

            if (result.Success)
            {
                payment.Status = PaymentStatus.Paid;
                payment.TransactionId = result.TransactionId;
                order.ProcessPayment(PaymentStatus.Paid);
                _logger.LogInformation("Payment processed successfully for order {OrderNumber}", order.OrderNumber);
            }
            else
            {
                payment.Status = PaymentStatus.Failed;
                order.ProcessPayment(PaymentStatus.Failed);
                _logger.LogWarning("Payment failed for order {OrderNumber}: {Error}", order.OrderNumber, result.ErrorMessage);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment for order {OrderNumber}", order.OrderNumber);
            throw;
        }
    }

    public async Task<bool> CanShipOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
        // Check if order is confirmed
        if (order.Status != OrderStatus.Confirmed)
            return false;

        // Check if payment is completed
        if (order.PaymentStatus != PaymentStatus.Paid)
            return false;

        // Check if all items are available in inventory
        foreach (var item in order.OrderItems)
        {
            var product = await _context.Products
                .Include(p => p.Inventory)
                .FirstOrDefaultAsync(p => p.Id == item.ProductId, cancellationToken);

            if (product?.Inventory < item.Quantity)
                return false;
        }

        return true;
    }

    public async Task UpdateInventoryForOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
        foreach (var item in order.OrderItems)
        {
            var product = await _context.Products
                .Include(p => p.Inventory)
                .FirstOrDefaultAsync(p => p.Id == item.ProductId, cancellationToken);

            if (product != null)
            {
                product.Inventory -= item.Quantity;
                _logger.LogInformation("Updated inventory for product {ProductId}: -{Quantity}", product.Id, item.Quantity);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
} 