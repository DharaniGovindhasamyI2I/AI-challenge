using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Events;

public class OrderCancelledEvent : BaseEvent
{
    public OrderCancelledEvent(Order order)
    {
        Order = order;
    }

    public Order Order { get; }
} 