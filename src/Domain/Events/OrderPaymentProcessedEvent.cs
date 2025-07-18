using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Events;

public class OrderPaymentProcessedEvent : BaseEvent
{
    public OrderPaymentProcessedEvent(Order order, PaymentStatus paymentStatus)
    {
        Order = order;
        PaymentStatus = paymentStatus;
    }

    public Order Order { get; }
    public PaymentStatus PaymentStatus { get; }
} 