using MediatR;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        // TODO: Add delete logic
        return Unit.Value;
    }
} 