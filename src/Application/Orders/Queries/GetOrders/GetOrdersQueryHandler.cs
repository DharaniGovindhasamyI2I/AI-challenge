using MediatR;
using CleanArchitecture.Application.Common.Interfaces;
using System.Collections.Generic;
using CleanArchitecture.Application.Orders.Queries.GetOrderById;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Orders.Common;
using CleanArchitecture.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, PaginatedList<OrderDto>>
{
    private readonly IApplicationDbContext _context;

    public GetOrdersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.Customer)
            .AsQueryable();

        // Apply filters
        if (request.CustomerId.HasValue)
            query = query.Where(o => o.CustomerId == request.CustomerId.Value);

        if (request.Status.HasValue)
            query = query.Where(o => o.Status == request.Status.Value);

        if (request.PaymentStatus.HasValue)
            query = query.Where(o => o.PaymentStatus == request.PaymentStatus.Value);

        if (request.FromDate.HasValue)
            query = query.Where(o => o.Created >= request.FromDate.Value);

        if (request.ToDate.HasValue)
            query = query.Where(o => o.Created <= request.ToDate.Value);

        if (!string.IsNullOrWhiteSpace(request.OrderNumber))
            query = query.Where(o => o.OrderNumber.Contains(request.OrderNumber));

        // Apply sorting
        query = request.SortBy?.ToLower() switch
        {
            "orderdate" or "created" => request.IsDescending 
                ? query.OrderByDescending(o => o.Created)
                : query.OrderBy(o => o.Created),
            "totalamount" => request.IsDescending
                ? query.OrderByDescending(o => o.TotalAmount.Amount)
                : query.OrderBy(o => o.TotalAmount.Amount),
            "status" => request.IsDescending
                ? query.OrderByDescending(o => o.Status)
                : query.OrderBy(o => o.Status),
            _ => request.IsDescending
                ? query.OrderByDescending(o => o.Created)
                : query.OrderBy(o => o.Created)
        };

        var orders = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var totalCount = await query.CountAsync(cancellationToken);

        var orderDtos = orders.Select(o => new OrderDto
        {
            Id = o.Id,
            OrderNumber = o.OrderNumber,
            CustomerId = o.CustomerId,
            Status = o.Status,
            PaymentStatus = o.PaymentStatus,
            TotalAmount = o.TotalAmount.Amount,
            ShippingAddress = new AddressDto
            {
                Street = o.ShippingAddress.Street,
                City = o.ShippingAddress.City,
                State = o.ShippingAddress.State,
                ZipCode = o.ShippingAddress.PostalCode,
                Country = o.ShippingAddress.Country
            },
            Notes = o.Notes,
            Items = o.OrderItems.Select(i => new OrderItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                UnitPrice = i.UnitPrice.Amount,
                Quantity = i.Quantity,
                TotalAmount = i.TotalAmount.Amount
            }).ToList(),
            Created = o.Created.DateTime,
            LastModified = o.LastModified.DateTime
        }).ToList();

        return new PaginatedList<OrderDto>(orderDtos, totalCount, request.PageNumber, request.PageSize);
    }
} 