using MediatR;

namespace CleanArchitecture.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand(int Id) : IRequest<Unit>; 