using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Entities;

public class Inventory : BaseAuditableEntity
{
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
} 