using System;

namespace ECommerceApi.Services
{
    public static class EmailService
    {
        public static void Send(string to, string subject, string body)
        {
            // Implement actual email sending here (SMTP, SendGrid, etc.)
            Console.WriteLine($"Email sent to {to}: {subject} - {body}");
        }
    }
}
