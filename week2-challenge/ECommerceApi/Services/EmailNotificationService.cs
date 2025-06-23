using ECommerceApi.Events;
using System.Net;
using System.Net.Mail;
using ECommerceApi.Services;
using System;

namespace ECommerceApi.Services
{
public class EmailNotificationService
{
    public void Subscribe(PaymentService paymentService)
    {
        paymentService.PaymentProcessed += OnPaymentProcessed;
    }

    private void OnPaymentProcessed(object sender, PaymentEventArgs e)
    {
        // Send email using SMTP
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("dharani.govindhasamy@ideas2it.com", "vgvnqbcszgsdzhht"),
            EnableSsl = true,
        };

        string subject = e.Status == "success" ? "Payment Successful" : "Payment Failed";
        string body = $"Your payment for {e.Product} (${e.Price}) was a {e.Status}.";

        var mailMessage = new MailMessage
        {
            From = new MailAddress("dharani.govindhasamy@ideas2it.com"),
            Subject = subject,
            Body = body,
            IsBodyHtml = false,
        };
        mailMessage.To.Add(e.Email);
        smtpClient.Send(mailMessage);
    }
}
}