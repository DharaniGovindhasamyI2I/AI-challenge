using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.ValueObjects;

public class Money : ValueObject
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "USD";

    public Money(decimal amount, string currency = "USD")
    {
        Amount = amount;
        Currency = currency;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Amount} {Currency}";
} 