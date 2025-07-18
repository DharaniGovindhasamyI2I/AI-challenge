using MediatR;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Orders.Common;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly IApplicationDbContext _context;

    public GetOrderByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

        if (order == null)
            return null;

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