using System;
using ECommerceApi.Models;
using ECommerceApi.Models.Payment;

namespace ECommerceApi.Models
{
    public class AdminDashboardNotifier : IObserver
    {
        public void Update(string message, string email)
        {
            // Simulate admin dashboard notification
            Console.WriteLine($"Admin dashboard notified: {message}");
        }
    }
} 