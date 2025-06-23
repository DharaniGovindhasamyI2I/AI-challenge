using ECommerceApi.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApi.Services
{
public class PaymentService
{
    // Declare the event using EventHandler<T>
    public event EventHandler<PaymentEventArgs> PaymentProcessed;

    public void ProcessPayment(string email, string product, decimal price, bool isSuccess)
    {
        // ... your payment logic here ...

        // After payment, raise the event
        OnPaymentProcessed(new PaymentEventArgs
        {
            Email = email,
            Product = product,
            Price = price,
            Status = isSuccess ? "success" : "failure"
        });
    }

    protected virtual void OnPaymentProcessed(PaymentEventArgs e)
    {
        PaymentProcessed?.Invoke(this, e);
    }
}
}
