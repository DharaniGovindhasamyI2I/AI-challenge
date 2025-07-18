using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IOrderService
{
    Task ValidateOrderAsync(Order order, CancellationToken cancellationToken = default);
    Task<bool> CanProcessPaymentAsync(Order order, CancellationToken cancellationToken = default);
    Task ProcessPaymentAsync(Order order, CancellationToken cancellationToken = default);
    Task<bool> CanShipOrderAsync(Order order, CancellationToken cancellationToken = default);
    Task UpdateInventoryForOrderAsync(Order order, CancellationToken cancellationToken = default);
} 