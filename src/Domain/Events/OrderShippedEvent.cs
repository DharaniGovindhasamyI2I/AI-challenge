using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Events;

public class OrderShippedEvent : BaseEvent
{
    public OrderShippedEvent(Order order)
    {
        Order = order;
    }

    public Order Order { get; }
} 