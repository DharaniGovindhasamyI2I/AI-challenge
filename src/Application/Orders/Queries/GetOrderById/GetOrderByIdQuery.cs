using CleanArchitecture.Application.Orders.Common;
using MediatR;

namespace CleanArchitecture.Application.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(int Id) : IRequest<OrderDto?>; 