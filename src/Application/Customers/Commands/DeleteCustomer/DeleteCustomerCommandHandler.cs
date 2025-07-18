using MediatR;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        // TODO: Add delete logic
        return Unit.Value;
    }
} 