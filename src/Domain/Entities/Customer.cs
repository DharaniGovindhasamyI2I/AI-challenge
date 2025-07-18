using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.ValueObjects;

namespace CleanArchitecture.Domain.Entities;

public class Customer : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Address Address { get; set; } = default!;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
} 