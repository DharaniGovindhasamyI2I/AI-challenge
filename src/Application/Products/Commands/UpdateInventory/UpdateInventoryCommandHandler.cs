using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Application.Products.Common;

namespace CleanArchitecture.Application.Products.Commands.UpdateInventory
{
    public class UpdateInventoryCommandHandler : IRequestHandler<UpdateInventoryCommand, ProductDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateInventoryCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(UpdateInventoryCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);
            if (product == null)
                throw new KeyNotFoundException($"Product with Id {request.ProductId} not found.");

            product.Inventory = request.NewInventory;
            product.AddUpdatedEvent();
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ProductDto>(product);
        }
    }
} 