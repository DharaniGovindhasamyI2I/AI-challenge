using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.ValueObjects;

namespace CleanArchitecture.Domain.Entities;

public class Payment : BaseAuditableEntity
{
    public int OrderId { get; set; }
    public Order? Order { get; set; }
    public Money Amount { get; set; } = default!;
    public DateTime PaymentDate { get; set; }
    public PaymentStatus Status { get; set; }
    public string? TransactionId { get; set; }
} 