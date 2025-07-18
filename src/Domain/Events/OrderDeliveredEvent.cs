using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Events;

public class OrderDeliveredEvent : BaseEvent
{
    public OrderDeliveredEvent(Order order)
    {
        Order = order;
    }

    public Order Order { get; }
} 