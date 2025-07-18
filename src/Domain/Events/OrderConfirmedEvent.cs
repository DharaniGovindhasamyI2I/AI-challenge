using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Events;

public class OrderConfirmedEvent : BaseEvent
{
    public OrderConfirmedEvent(Order order)
    {
        Order = order;
    }

    public Order Order { get; }
} 