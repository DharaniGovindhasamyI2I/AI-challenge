using MediatR;
using CleanArchitecture.Application.Products.Common;

namespace CleanArchitecture.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    string Currency,
    int Inventory,
    int CategoryId
) : IRequest<ProductDto>; 