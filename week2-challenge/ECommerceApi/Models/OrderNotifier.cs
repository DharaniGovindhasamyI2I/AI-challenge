using System.Collections.Generic;
using ECommerceApi.Models;
using ECommerceApi.Services;

namespace ECommerceApi.Models
{
    public class OrderNotifier
    {
        private readonly List<IObserver> _observers = new();
        public void Attach(IObserver observer) => _observers.Add(observer);
        public void Detach(IObserver observer) => _observers.Remove(observer);
        public void Notify(string email, string message)
        {
            foreach (var observer in _observers)
                observer.Update(email, message);
        }

        public void Update(string email, string message)
        {
            // Use the 'email' and 'message' parameters here
            Console.WriteLine($"Order notification sent to {email}: {message}");
        }
    }
} 