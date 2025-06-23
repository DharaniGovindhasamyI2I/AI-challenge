  namespace ECommerceApi.Models.Payment.Strategies
{
public interface IPaymentStrategy
{
    bool Pay(PaymentRequest request);
}
}