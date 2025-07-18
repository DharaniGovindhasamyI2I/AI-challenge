using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Orders.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, OrderDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateOrderStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OrderDto> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order == null)
            throw new ValidationException($"Order with ID {request.OrderId} not found.");

        // Apply state machine logic
        switch (request.NewStatus)
        {
            case OrderStatus.Confirmed:
                order.Confirm();
                break;
            case OrderStatus.Shipped:
                order.Ship();
                break;
            case OrderStatus.Delivered:
                order.Deliver();
                break;
            case OrderStatus.Cancelled:
                order.Cancel();
                break;
            default:
                throw new ValidationException($"Invalid status transition from {order.Status} to {request.NewStatus}");
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            CustomerId = order.CustomerId,
            Status = order.Status,
            PaymentStatus = order.PaymentStatus,
            TotalAmount = order.TotalAmount.Amount,
            ShippingAddress = new AddressDto
            {
                Street = order.ShippingAddress.Street,
                City = order.ShippingAddress.City,
                State = order.ShippingAddress.State,
                ZipCode = order.ShippingAddress.PostalCode,
                Country = order.ShippingAddress.Country
            },
            Notes = order.Notes,
            Items = order.OrderItems.Select(i => new OrderItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                UnitPrice = i.UnitPrice.Amount,
                Quantity = i.Quantity,
                TotalAmount = i.TotalAmount.Amount
            }).ToList(),
            Created = order.Created.DateTime,
            LastModified = order.LastModified.DateTime
        };
    }
} 