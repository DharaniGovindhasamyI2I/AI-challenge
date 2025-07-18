using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Orders.Commands.CreateOrder;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace CleanArchitecture.Application.FunctionalTests.Orders.Commands;

using static Testing;

public class CreateOrderTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateOrderCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidCustomer()
    {
        var command = new CreateOrderCommand
        {
            CustomerId = 999, // Non-existent customer
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

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldRequireValidProduct()
    {
        // Arrange - Create customer but not product
        var customer = new Customer("Test Customer", "test@example.com", "1234567890");
        await AddAsync(customer);

        var command = new CreateOrderCommand
        {
            CustomerId = customer.Id,
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
                new() { ProductId = 999, Quantity = 2 } // Non-existent product
            }
        };

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldCreateOrder()
    {
        // Arrange - Create test data
        var customer = new Customer("Test Customer", "test@example.com", "1234567890");
        await AddAsync(customer);

        var product = new Product("Test Product", "Test Description", 10.99m, 100);
        await AddAsync(product);

        var command = new CreateOrderCommand
        {
            CustomerId = customer.Id,
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
                new() { ProductId = product.Id, Quantity = 2 }
            },
            Notes = "Test order"
        };

        // Act
        var orderId = await SendAsync(command);

        // Assert
        var order = await FindAsync<Order>(orderId);
        order.Should().NotBeNull();
        order!.CustomerId.Should().Be(customer.Id);
        order.ShippingAddress.Street.Should().Be("123 Test St");
        order.Notes.Should().Be("Test order");
        order.Status.Should().Be(OrderStatus.Pending);
        order.Items.Should().HaveCount(1);
        order.Items.First().ProductId.Should().Be(product.Id);
        order.Items.First().Quantity.Should().Be(2);
    }

    [Test]
    public async Task ShouldCalculateOrderTotal()
    {
        // Arrange - Create test data
        var customer = new Customer("Test Customer", "test@example.com", "1234567890");
        await AddAsync(customer);

        var product1 = new Product("Product 1", "Description 1", 10.00m, 100);
        var product2 = new Product("Product 2", "Description 2", 15.50m, 50);
        await AddAsync(product1);
        await AddAsync(product2);

        var command = new CreateOrderCommand
        {
            CustomerId = customer.Id,
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
                new() { ProductId = product1.Id, Quantity = 2 }, // 2 * 10.00 = 20.00
                new() { ProductId = product2.Id, Quantity = 1 }  // 1 * 15.50 = 15.50
            }
        };

        // Act
        var orderId = await SendAsync(command);

        // Assert
        var order = await FindAsync<Order>(orderId);
        order.Should().NotBeNull();
        order!.Total.Should().Be(35.50m); // 20.00 + 15.50
    }
} 