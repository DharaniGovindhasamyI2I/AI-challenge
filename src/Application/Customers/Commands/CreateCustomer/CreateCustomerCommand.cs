using MediatR;

namespace CleanArchitecture.Application.Customers.Commands.CreateCustomer;

public record CreateCustomerCommand(string Name, string Email, string Street, string City, string State, string PostalCode, string Country) : IRequest<int>; 