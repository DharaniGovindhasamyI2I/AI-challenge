using System.Collections.Generic;

namespace CleanArchitecture.Application.Customers.Queries.GetCustomerById;

public class CustomerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public List<CustomerOrderDto> Orders { get; set; } = new();
}

public class CustomerOrderDto
{
    public int OrderId { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = string.Empty;
} 