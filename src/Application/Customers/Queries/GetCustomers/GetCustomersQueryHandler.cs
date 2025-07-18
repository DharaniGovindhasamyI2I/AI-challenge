using MediatR;
using CleanArchitecture.Application.Common.Interfaces;
using System.Collections.Generic;
using CleanArchitecture.Application.Customers.Queries.GetCustomerById;

namespace CleanArchitecture.Application.Customers.Queries.GetCustomers;

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, List<CustomerDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCustomersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        // TODO: Add fetching and mapping logic
        return new List<CustomerDto>();
    }
} 