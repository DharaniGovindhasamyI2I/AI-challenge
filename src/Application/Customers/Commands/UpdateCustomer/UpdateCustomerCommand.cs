using MediatR;

namespace CleanArchitecture.Application.Customers.Commands.UpdateCustomer;

public record UpdateCustomerCommand(int Id, string Name, string Email, string Street, string City, string State, string PostalCode, string Country) : IRequest<Unit>; 