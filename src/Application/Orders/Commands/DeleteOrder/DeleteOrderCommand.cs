using MediatR;

namespace CleanArchitecture.Application.Orders.Commands.DeleteOrder;

public record DeleteOrderCommand(int Id) : IRequest<Unit>; 