using MediatR;
using CleanArchitecture.Application.Products.Common;

namespace CleanArchitecture.Application.Products.Commands.UpdateInventory
{
    public class UpdateInventoryCommand : IRequest<ProductDto>
    {
        public int ProductId { get; set; }
        public int NewInventory { get; set; }
    }
} 