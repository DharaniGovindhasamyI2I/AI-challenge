using MediatR;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Orders.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IOrderService _orderService;

    public CreateOrderCommandHandler(IApplicationDbContext context, IOrderService orderService)
    {
        _context = context;
        _orderService = orderService;
    }

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Validate customer exists
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == request.CustomerId, cancellationToken);
        
        if (customer == null)
            throw new ValidationException($"Customer with ID {request.CustomerId} not found.");

        // Validate all products exist and have sufficient inventory
        var productIds = request.Items.Select(i => i.ProductId).ToList();
        var products = await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync(cancellationToken);

        if (products.Count != productIds.Count)
            throw new ValidationException("One or more products not found.");

        // Check inventory levels
        foreach (var item in request.Items)
        {
            var product = products.First(p => p.Id == item.ProductId);
            if (product.Inventory < item.Quantity)
                throw new ValidationException($"Insufficient inventory for product {product.Name}. Available: {product.Inventory}, Requested: {item.Quantity}");
        }

        // Create shipping address
        var shippingAddress = new Address(
            request.ShippingAddress.Street,
            request.ShippingAddress.City,
            request.ShippingAddress.State,
            request.ShippingAddress.ZipCode,
            request.ShippingAddress.Country);

        // Create order
        var order = new Order(request.CustomerId, shippingAddress, request.Notes);

        // Add items to order
        foreach (var item in request.Items)
        {
            var product = products.First(p => p.Id == item.ProductId);
            order.AddItem(product, item.Quantity);
        }

        // Validate order with business rules
        await _orderService.ValidateOrderAsync(order, cancellationToken);

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        // Return the created order
        return new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            CustomerId = order.CustomerId,
            Status = order.Status,
            PaymentStatus = order.PaymentStatus,
            TotalAmount = order.TotalAmount.Amount,
            ShippingAddress = new Application.Orders.Common.AddressDto
            {
                Street = order.ShippingAddress.Street,
                City = order.ShippingAddress.City,
                State = order.ShippingAddress.State,
                ZipCode = order.ShippingAddress.PostalCode,
                Country = order.ShippingAddress.Country
            },
            Notes = order.Notes,
            Items = order.OrderItems.Select(i => new Application.Orders.Common.OrderItemDto
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