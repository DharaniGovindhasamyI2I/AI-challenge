using System;
using ECommerceApi.Models.Payment.Strategies;
using ECommerceApi.Models;

namespace ECommerceApi.Models.Payment
{
public class PaymentContext
{
    private IPaymentStrategy _strategy;

    public void SetStrategy(IPaymentStrategy strategy)
    {
        _strategy = strategy;
    }

    public bool Pay(PaymentRequest request)
    {
        if (_strategy == null) throw new InvalidOperationException("Payment strategy not set.");
        return _strategy.Pay(request);
    }
}
}
