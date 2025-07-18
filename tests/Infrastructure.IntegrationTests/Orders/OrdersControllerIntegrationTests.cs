using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.IntegrationTests.TestContainers;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.ValueObjects;
using CleanArchitecture.Domain.Enums;
using System.Text.Json;
using System.Text;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.Infrastructure.IntegrationTests.Orders;

public class OrdersControllerIntegrationTests : IClassFixture<TestContainersFixture>, IAsyncDisposable
{
    private readonly TestContainersFixture _fixture;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public OrdersControllerIntegrationTests(TestContainersFixture fixture)
    {
        _fixture = fixture;
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Replace database context with test container
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(_fixture.PostgreSqlContainer.GetConnectionString()));

                    // Replace Redis with test container
                    var redisDescriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(IDistributedCache));
                    if (redisDescriptor != null)
                        services.Remove(redisDescriptor);

                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = _fixture.RedisContainer.GetConnectionString();
                        options.InstanceName = "Test_";
                    });
                });
            });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetOrders_ShouldReturnPaginatedList()
    {
        // Arrange - Create test data
        var orderId = await SeedTestData();

        // Act
        var response = await _client.GetAsync("/api/orders?pageNumber=1&pageSize=10");

        // Assert
        response.Should().BeSuccessful();
        var responseContent = await response.Content.ReadAsStringAsync();
        var orders = JsonSerializer.Deserialize<dynamic>(responseContent);
        orders.Should().NotBeNull();
        orders!.GetProperty("items").GetArrayLength().Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetOrders_WithFilters_ShouldReturnFilteredResults()
    {
        // Arrange - Create test data
        var orderId = await SeedTestData();

        // Act
        var response = await _client.GetAsync("/api/orders?status=Pending&pageNumber=1&pageSize=10");

        // Assert
        response.Should().BeSuccessful();
        var responseContent = await response.Content.ReadAsStringAsync();
        var orders = JsonSerializer.Deserialize<dynamic>(responseContent);
        orders.Should().NotBeNull();
    }

    [Fact]
    public async Task GetOrderById_ShouldReturnOrder()
    {
        // Arrange - Create test data and get order ID
        var orderId = await SeedTestData();

        // Act
        var response = await _client.GetAsync($"/api/orders/{orderId}");

        // Assert
        response.Should().BeSuccessful();
        var responseContent = await response.Content.ReadAsStringAsync();
        var order = JsonSerializer.Deserialize<dynamic>(responseContent);
        order.Should().NotBeNull();
        order!.GetProperty("id").GetInt32().Should().Be(orderId);
    }

    [Fact]
    public async Task GetOrderById_WithInvalidId_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/orders/999");

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateOrder_ShouldReturnCreatedOrder()
    {
        // Arrange
        var createOrderRequest = new
        {
            CustomerId = 1,
            ShippingAddress = new
            {
                Street = "123 Test St",
                City = "Test City",
                State = "TS",
                ZipCode = "12345",
                Country = "Test Country"
            },
            Items = new[]
            {
                new
                {
                    ProductId = 1,
                    Quantity = 2
                }
            },
            Notes = "Test order"
        };

        var json = JsonSerializer.Serialize(createOrderRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/orders", content);

        // Assert
        response.Should().BeSuccessful();
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.Created);
        var responseContent = await response.Content.ReadAsStringAsync();
        var order = JsonSerializer.Deserialize<dynamic>(responseContent);
        order.Should().NotBeNull();
        order!.GetProperty("status").GetString().Should().Be("Pending");
    }

    [Fact]
    public async Task UpdateOrderStatus_ShouldUpdateStatus()
    {
        // Arrange - Create test data
        var orderId = await SeedTestData();
        var updateStatusRequest = new
        {
            OrderId = orderId,
            NewStatus = "Confirmed"
        };

        var json = JsonSerializer.Serialize(updateStatusRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/orders/{orderId}/status", content);

        // Assert
        response.Should().BeSuccessful();
        var responseContent = await response.Content.ReadAsStringAsync();
        var order = JsonSerializer.Deserialize<dynamic>(responseContent);
        order.Should().NotBeNull();
        order!.GetProperty("status").GetString().Should().Be("Confirmed");
    }

    [Fact]
    public async Task UpdateOrderStatus_WithMismatchedId_ShouldReturnBadRequest()
    {
        // Arrange
        var orderId = await SeedTestData();
        var updateStatusRequest = new
        {
            OrderId = orderId + 1, // Mismatched ID
            NewStatus = "Confirmed"
        };

        var json = JsonSerializer.Serialize(updateStatusRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/orders/{orderId}/status", content);

        // Assert
        response.Should().HaveStatusCode(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ConfirmOrder_ShouldUpdateStatusToConfirmed()
    {
        // Arrange - Create test data
        var orderId = await SeedTestData();

        // Act
        var response = await _client.PutAsync($"/api/orders/{orderId}/confirm", null);

        // Assert
        response.Should().BeSuccessful();
        var responseContent = await response.Content.ReadAsStringAsync();
        var order = JsonSerializer.Deserialize<dynamic>(responseContent);
        order.Should().NotBeNull();
        order!.GetProperty("status").GetString().Should().Be("Confirmed");
    }

    [Fact]
    public async Task ShipOrder_ShouldUpdateStatusToShipped()
    {
        // Arrange - Create test data
        var orderId = await SeedTestData();

        // Act
        var response = await _client.PutAsync($"/api/orders/{orderId}/ship", null);

        // Assert
        response.Should().BeSuccessful();
        var responseContent = await response.Content.ReadAsStringAsync();
        var order = JsonSerializer.Deserialize<dynamic>(responseContent);
        order.Should().NotBeNull();
        order!.GetProperty("status").GetString().Should().Be("Shipped");
    }

    [Fact]
    public async Task DeliverOrder_ShouldUpdateStatusToDelivered()
    {
        // Arrange - Create test data
        var orderId = await SeedTestData();

        // Act
        var response = await _client.PutAsync($"/api/orders/{orderId}/deliver", null);

        // Assert
        response.Should().BeSuccessful();
        var responseContent = await response.Content.ReadAsStringAsync();
        var order = JsonSerializer.Deserialize<dynamic>(responseContent);
        order.Should().NotBeNull();
        order!.GetProperty("status").GetString().Should().Be("Delivered");
    }

    [Fact]
    public async Task CancelOrder_ShouldUpdateStatusToCancelled()
    {
        // Arrange - Create test data
        var orderId = await SeedTestData();

        // Act
        var response = await _client.PutAsync($"/api/orders/{orderId}/cancel", null);

        // Assert
        response.Should().BeSuccessful();
        var responseContent = await response.Content.ReadAsStringAsync();
        var order = JsonSerializer.Deserialize<dynamic>(responseContent);
        order.Should().NotBeNull();
        order!.GetProperty("status").GetString().Should().Be("Cancelled");
    }

    private async Task<int> SeedTestData()
    {
        using var scope = _fixture.Host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Create test customer
        var customer = new Customer("Test Customer", "test@example.com", "1234567890");
        context.Customers.Add(customer);

        // Create test product
        var product = new Product("Test Product", "Test Description", 10.99m, 100);
        context.Products.Add(product);

        await context.SaveChangesAsync();

        // Create test order
        var shippingAddress = new Address("123 Test St", "Test City", "TS", "12345", "Test Country");
        var order = new Order(customer.Id, shippingAddress, "Test order");
        order.AddItem(product, 2);
        context.Orders.Add(order);

        await context.SaveChangesAsync();

        return order.Id;
    }

    public async ValueTask DisposeAsync()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }
} 