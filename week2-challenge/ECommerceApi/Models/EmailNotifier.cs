using System;
using ECommerceApi.Models;
using ECommerceApi.Services;

namespace ECommerceApi.Models
{
    public class EmailNotifier : IObserver
    {
        public void Update(string email, string message)
        {
            // Use your email service here (e.g., SMTP, SendGrid, etc.)
            EmailService.Send(email, "Payment Notification", message);
        }
    }
} 