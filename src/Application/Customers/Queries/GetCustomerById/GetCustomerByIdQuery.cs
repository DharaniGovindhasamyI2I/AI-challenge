using MediatR;

namespace CleanArchitecture.Application.Customers.Queries.GetCustomerById;

public record GetCustomerByIdQuery(int Id) : IRequest<CustomerDto>; 