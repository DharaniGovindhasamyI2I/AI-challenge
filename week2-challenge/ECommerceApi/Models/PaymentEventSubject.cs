using System.Collections.Generic;
using ECommerceApi.Models.Payment;
using ECommerceApi.Models;

namespace ECommerceApi.Models
{
public class PaymentEventSubject : ISubject
{
    private readonly List<IObserver> _observers = new();

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);

    public void Notify(string email, string message)
    {
        foreach (var observer in _observers)
        {
            observer.Update(email, message);
        }
    }
}
}