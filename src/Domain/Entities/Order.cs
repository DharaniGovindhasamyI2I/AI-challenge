using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Events;
using CleanArchitecture.Domain.ValueObjects;

namespace CleanArchitecture.Domain.Entities;

public class Order : BaseAuditableEntity
{
    public string OrderNumber { get; private set; } = string.Empty;
    public int CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public OrderStatus Status { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }
    public Money TotalAmount { get; private set; } = null!;
    public Address ShippingAddress { get; private set; } = null!;
    public string? Notes { get; private set; }
    
    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private Order() { } // For EF Core

    public Order(int customerId, Address shippingAddress, string? notes = null)
    {
        CustomerId = customerId;
        ShippingAddress = shippingAddress;
        Notes = notes;
        OrderNumber = GenerateOrderNumber();
        Status = OrderStatus.Pending;
        PaymentStatus = PaymentStatus.Pending;
        TotalAmount = new Money(0);

        AddDomainEvent(new OrderCreatedEvent(this));
    }

    public void AddItem(Product product, int quantity)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));
        
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Cannot add items to an order that is not pending.");

        var existingItem = _orderItems.FirstOrDefault(i => i.ProductId == product.Id);
        
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            var orderItem = new OrderItem(this, product.Id, product.Name, new Money(product.Price), quantity);
            _orderItems.Add(orderItem);
        }

        RecalculateTotal();
    }

    public void RemoveItem(int productId)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Cannot remove items from an order that is not pending.");

        var item = _orderItems.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            _orderItems.Remove(item);
            RecalculateTotal();
        }
    }

    public void UpdateItemQuantity(int productId, int quantity)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Cannot update items in an order that is not pending.");

        var item = _orderItems.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            item.UpdateQuantity(quantity);
            RecalculateTotal();
        }
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Only pending orders can be confirmed.");

        if (!_orderItems.Any())
            throw new InvalidOperationException("Cannot confirm an order without items.");

        Status = OrderStatus.Confirmed;
        AddDomainEvent(new OrderConfirmedEvent(this));
    }

    public void Ship()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException("Only confirmed orders can be shipped.");

        if (PaymentStatus != PaymentStatus.Paid)
            throw new InvalidOperationException("Cannot ship an order that has not been paid.");

        Status = OrderStatus.Shipped;
        AddDomainEvent(new OrderShippedEvent(this));
    }

    public void Deliver()
    {
        if (Status != OrderStatus.Shipped)
            throw new InvalidOperationException("Only shipped orders can be delivered.");

        Status = OrderStatus.Delivered;
        AddDomainEvent(new OrderDeliveredEvent(this));
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Delivered)
            throw new InvalidOperationException("Cannot cancel a delivered order.");

        Status = OrderStatus.Cancelled;
        AddDomainEvent(new OrderCancelledEvent(this));
    }

    public void ProcessPayment(PaymentStatus paymentStatus)
    {
        PaymentStatus = paymentStatus;
        AddDomainEvent(new OrderPaymentProcessedEvent(this, paymentStatus));
    }

    private void RecalculateTotal()
    {
        TotalAmount = new Money(_orderItems.Sum(i => i.TotalAmount.Amount));
    }

    private static string GenerateOrderNumber()
    {
        return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
    }
} 