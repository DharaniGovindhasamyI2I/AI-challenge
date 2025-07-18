using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Orders.Commands.CreateOrder;
using FluentAssertions;
using NUnit.Framework;

namespace CleanArchitecture.Application.UnitTests.Orders.Commands;

public class CreateOrderCommandTests
{
    [Test]
    public async Task ShouldRequireCustomerId()
    {
        var command = new CreateOrderCommand
        {
            CustomerId = 0,
            ShippingAddress = new AddressDto
            {
                Street = "123 Test St",
                City = "Test City",
                State = "TS",
                ZipCode = "12345",
                Country = "Test Country"
            },
            Items = new List<OrderItemDto>
            {
                new() { ProductId = 1, Quantity = 2 }
            }
        };

        await FluentActions.Invoking(() => 
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidShippingAddress()
    {
        var command = new CreateOrderCommand
        {
            CustomerId = 1,
            ShippingAddress = new AddressDto
            {
                Street = "", // Invalid - empty street
                City = "Test City",
                State = "TS",
                ZipCode = "12345",
                Country = "Test Country"
            },
            Items = new List<OrderItemDto>
            {
                new() { ProductId = 1, Quantity = 2 }
            }
        };

        await FluentActions.Invoking(() => 
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireAtLeastOneItem()
    {
        var command = new CreateOrderCommand
        {
            CustomerId = 1,
            ShippingAddress = new AddressDto
            {
                Street = "123 Test St",
                City = "Test City",
                State = "TS",
                ZipCode = "12345",
                Country = "Test Country"
            },
            Items = new List<OrderItemDto>() // Empty items list
        };

        await FluentActions.Invoking(() => 
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidItemQuantity()
    {
        var command = new CreateOrderCommand
        {
            CustomerId = 1,
            ShippingAddress = new AddressDto
            {
                Street = "123 Test St",
                City = "Test City",
                State = "TS",
                ZipCode = "12345",
                Country = "Test Country"
            },
            Items = new List<OrderItemDto>
            {
                new() { ProductId = 1, Quantity = 0 } // Invalid quantity
            }
        };

        await FluentActions.Invoking(() => 
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }
} 