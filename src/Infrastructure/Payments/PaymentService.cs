using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Infrastructure.Payments
{
    public class PaymentService : IPaymentService
    {
        public async Task<PaymentResult> ProcessPaymentAsync(Payment payment, CancellationToken cancellationToken)
        {
            // Simulate payment processing
            await Task.Delay(500, cancellationToken);
            return new PaymentResult
            {
                Success = true,
                TransactionId = Guid.NewGuid().ToString(),
                ErrorMessage = null
            };
        }
    }
} 