using CleanArchitecture.Application.Orders.Common;
using CleanArchitecture.Domain.Enums;
using MediatR;

namespace CleanArchitecture.Application.Orders.Commands.UpdateOrderStatus;

public record UpdateOrderStatusCommand : IRequest<OrderDto>
{
    public int OrderId { get; init; }
    public OrderStatus NewStatus { get; init; }
} 