using MediatR;

namespace CleanArchitecture.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(int Id, string Name, string Description, decimal Price, int CategoryId, int Quantity) : IRequest<Unit>; 