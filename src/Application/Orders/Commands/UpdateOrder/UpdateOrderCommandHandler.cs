using MediatR;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        // TODO: Add update logic
        return Unit.Value;
    }
} 