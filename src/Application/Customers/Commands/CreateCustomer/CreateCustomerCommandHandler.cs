using MediatR;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        // TODO: Add customer creation logic
        return 0;
    }
} 