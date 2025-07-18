using MediatR;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        // TODO: Add update logic
        return Unit.Value;
    }
} 