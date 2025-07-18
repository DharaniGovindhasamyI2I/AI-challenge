using MediatR;
using System.Collections.Generic;

namespace CleanArchitecture.Application.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand(int Id, int CustomerId, List<UpdateOrderItemDto> Items, string Status) : IRequest<Unit>;

public class UpdateOrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
} 