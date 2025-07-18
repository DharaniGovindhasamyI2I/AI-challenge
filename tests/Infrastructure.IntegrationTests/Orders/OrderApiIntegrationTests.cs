using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.IntegrationTests.TestContainers;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.ValueObjects;
using System.Text.Json;
using System.Text;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.Infrastructure.IntegrationTests.Orders;

public class OrderApiIntegrationTests : IClassFixture<TestContainersFixture>, IAsyncDisposable
{
    private readonly TestContainersFixture _fixture;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public OrderApiIntegrationTests(TestContainersFixture fixture)
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
        var responseContent = await response.Content.ReadAsStringAsync();
        var order = JsonSerializer.Deserialize<dynamic>(responseContent);
        order.Should().NotBeNull();
    }

    [Fact]
    public async Task GetOrders_ShouldReturnOrdersList()
    {
        // Arrange - Create some test data
        await SeedTestData();

        // Act
        var response = await _client.GetAsync("/api/orders");

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