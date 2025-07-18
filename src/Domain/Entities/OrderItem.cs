using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.ValueObjects;

namespace CleanArchitecture.Domain.Entities;

public class OrderItem : BaseEntity
{
    public int OrderId { get; private set; }
    public Order Order { get; private set; } = null!;
    public int ProductId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public Money UnitPrice { get; private set; } = null!;
    public int Quantity { get; private set; }
    public Money TotalAmount { get; private set; } = null!;

    private OrderItem() { } // For EF Core

    public OrderItem(Order order, int productId, string productName, Money unitPrice, int quantity)
    {
        OrderId = order.Id;
        Order = order;
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
        CalculateTotal();
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));

        Quantity = newQuantity;
        CalculateTotal();
    }

    private void CalculateTotal()
    {
        TotalAmount = new Money(UnitPrice.Amount * Quantity);
    }
} 