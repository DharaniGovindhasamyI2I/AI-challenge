using MediatR;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        // TODO: Add delete logic
        return Unit.Value;
    }
} 