using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Orders.Common;
using CleanArchitecture.Domain.ValueObjects;
using MediatR;
using System.Collections.Generic;

namespace CleanArchitecture.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand : IRequest<OrderDto>
{
    public int CustomerId { get; init; }
    public AddressDto ShippingAddress { get; init; } = default!;
    public string? Notes { get; init; }
    public List<OrderItemDto> Items { get; init; } = new();
}

public record AddressDto
{
    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
}

public record OrderItemDto
{
    public int ProductId { get; init; }
    public int Quantity { get; init; }
} 