using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPaymentAsync(Payment payment, CancellationToken cancellationToken);
    }

    public class PaymentResult
    {
        public bool Success { get; set; }
        public string? TransactionId { get; set; }
        public string? ErrorMessage { get; set; }
    }
} 