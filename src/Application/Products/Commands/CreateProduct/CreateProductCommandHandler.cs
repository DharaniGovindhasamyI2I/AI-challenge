using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Products.Commands.CreateProduct;
using CleanArchitecture.Domain.Entities;
using MediatR;
using CleanArchitecture.Application.Products.Common;

namespace CleanArchitecture.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Currency = request.Currency,
                Inventory = request.Inventory,
                CategoryId = request.CategoryId
            };
            product.AddCreatedEvent();
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ProductDto>(product);
        }
    }
} 