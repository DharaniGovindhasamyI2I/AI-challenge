using ECommerceApi.Models.Payment;
using ECommerceApi.Models;
using ECommerceApi.Models.Payment.Strategies;

  namespace ECommerceApi.Models.Payment.Strategies
{

public class CreditCardPayment : IPaymentStrategy
{
    public bool Pay(PaymentRequest request)
    {
        // Simulate credit card payment logic
        return !string.IsNullOrEmpty(request.CardNumber) && !string.IsNullOrEmpty(request.CVV);
    }
}
}