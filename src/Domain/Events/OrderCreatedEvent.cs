using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Events;

public class OrderCreatedEvent : BaseEvent
{
    public OrderCreatedEvent(Order order)
    {
        Order = order;
    }

    public Order Order { get; }
} 