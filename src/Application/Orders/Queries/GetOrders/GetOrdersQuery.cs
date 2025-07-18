using MediatR;
using System.Collections.Generic;
using CleanArchitecture.Application.Orders.Queries.GetOrderById;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Orders.Common;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Orders.Queries.GetOrders;

public record GetOrdersQuery : IRequest<PaginatedList<OrderDto>>
{
    public int? CustomerId { get; init; }
    public OrderStatus? Status { get; init; }
    public PaymentStatus? PaymentStatus { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public string? OrderNumber { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortBy { get; init; }
    public bool IsDescending { get; init; } = true;
} 