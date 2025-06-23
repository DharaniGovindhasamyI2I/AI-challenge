using ECommerceApi.Models.Payment.Strategies;
using ECommerceApi.Models.Payment;


  namespace ECommerceApi.Models.Payment.Strategies
{
public class PayPalPayment : IPaymentStrategy
{
    public bool Pay(PaymentRequest request)
    {
        // Simulate PayPal payment logic
        return !string.IsNullOrEmpty(request.PayPalEmail);
    }
}
}
