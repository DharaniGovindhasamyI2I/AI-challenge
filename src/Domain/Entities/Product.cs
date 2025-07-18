using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Events;
using System.Collections.Generic;

namespace CleanArchitecture.Domain.Entities;

public class Product : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public int Inventory { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public void AddCreatedEvent()
    {
        this.AddDomainEvent(new ProductCreatedEvent(this));
    }

    public void AddUpdatedEvent()
    {
        this.AddDomainEvent(new ProductUpdatedEvent(this));
    }
} 