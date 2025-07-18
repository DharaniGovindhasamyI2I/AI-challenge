using MediatR;
using CleanArchitecture.Application.Products.Common;

namespace CleanArchitecture.Application.Products.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<ProductDto>
{
    public int Id { get; set; }
    public GetProductByIdQuery(int id) { Id = id; }
} 