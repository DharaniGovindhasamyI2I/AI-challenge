using ECommerceApi.Models.Payment;
using ECommerceApi.Models;
using ECommerceApi.Models.Payment.Strategies;

  namespace ECommerceApi.Models.Payment.Strategies
{

public class BankTransferPayment : IPaymentStrategy
{
    public bool Pay(PaymentRequest request)
    {
        // Simulate bank transfer logic
        return !string.IsNullOrEmpty(request.BankAccount) && !string.IsNullOrEmpty(request.BankName);
    }
}
}