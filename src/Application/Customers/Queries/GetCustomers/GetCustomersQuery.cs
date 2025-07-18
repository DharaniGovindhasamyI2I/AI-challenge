using MediatR;
using System.Collections.Generic;
using CleanArchitecture.Application.Customers.Queries.GetCustomerById;

namespace CleanArchitecture.Application.Customers.Queries.GetCustomers;

public record GetCustomersQuery() : IRequest<List<CustomerDto>>; 