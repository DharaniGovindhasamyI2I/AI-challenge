using MediatR;

namespace CleanArchitecture.Application.Customers.Commands.DeleteCustomer;

public record DeleteCustomerCommand(int Id) : IRequest<Unit>; 