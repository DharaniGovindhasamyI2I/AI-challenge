using MediatR;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly IApplicationDbContext _context;

    public GetCustomerByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        // TODO: Add fetching and mapping logic
        return null!;
    }
} 