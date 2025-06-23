
using ECommerceApi.Models;
using ECommerceApi.Models.Payment;


namespace ECommerceApi.Models
{
public class PaymentRequest
{
    public string Email { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } // "CreditCard", "PayPal", "BankTransfer"
    // Credit Card
    public string CardNumber { get; set; }
    public string CardHolder { get; set; }
    public string CVV { get; set; }
    // PayPal
    public string PayPalEmail { get; set; }
    // Bank Transfer
    public string BankAccount { get; set; }
    public string BankName { get; set; }
}
}
