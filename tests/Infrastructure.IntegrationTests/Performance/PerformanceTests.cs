using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.IntegrationTests.TestContainers;
using System.Diagnostics;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.Infrastructure.IntegrationTests.Performance;

public class PerformanceTests : IClassFixture<TestContainersFixture>, IAsyncDisposable
{
    private readonly TestContainersFixture _fixture;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public PerformanceTests(TestContainersFixture fixture)
    {
        _fixture = fixture;
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Configure test containers
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(_fixture.PostgreSqlContainer.GetConnectionString()));

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
    public async Task GetProducts_WithCaching_ShouldBeFasterOnSecondCall()
    {
        // Arrange
        await SeedTestData();

        // Act - First call (cache miss)
        var stopwatch1 = Stopwatch.StartNew();
        var response1 = await _client.GetAsync("/api/products");
        stopwatch1.Stop();

        // Act - Second call (cache hit)
        var stopwatch2 = Stopwatch.StartNew();
        var response2 = await _client.GetAsync("/api/products");
        stopwatch2.Stop();

        // Assert
        response1.Should().BeSuccessful();
        response2.Should().BeSuccessful();
        
        // Second call should be significantly faster
        stopwatch2.ElapsedMilliseconds.Should().BeLessThan(stopwatch1.ElapsedMilliseconds);
        
        // Log performance metrics
        Console.WriteLine($"First call (cache miss): {stopwatch1.ElapsedMilliseconds}ms");
        Console.WriteLine($"Second call (cache hit): {stopwatch2.ElapsedMilliseconds}ms");
        Console.WriteLine($"Performance improvement: {((double)(stopwatch1.ElapsedMilliseconds - stopwatch2.ElapsedMilliseconds) / stopwatch1.ElapsedMilliseconds * 100):F1}%");
    }

    [Fact]
    public async Task GetOrders_ShouldRespondWithinAcceptableTime()
    {
        // Arrange
        await SeedTestData();

        // Act
        var stopwatch = Stopwatch.StartNew();
        var response = await _client.GetAsync("/api/orders");
        stopwatch.Stop();

        // Assert
        response.Should().BeSuccessful();
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000); // Should respond within 1 second
        
        Console.WriteLine($"GetOrders response time: {stopwatch.ElapsedMilliseconds}ms");
    }

    [Fact]
    public async Task ConcurrentRequests_ShouldHandleLoad()
    {
        // Arrange
        await SeedTestData();
        var tasks = new List<Task<HttpResponseMessage>>();
        var stopwatch = Stopwatch.StartNew();

        // Act - Send 10 concurrent requests
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(_client.GetAsync("/api/products"));
        }

        var responses = await Task.WhenAll(tasks);
        stopwatch.Stop();

        // Assert
        responses.Should().AllSatisfy(r => r.Should().BeSuccessful());
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(5000); // Should complete within 5 seconds
        
        Console.WriteLine($"Concurrent requests completed in: {stopwatch.ElapsedMilliseconds}ms");
    }

    private async Task SeedTestData()
    {
        using var scope = _fixture.Host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Create test products
        for (int i = 1; i <= 10; i++)
        {
            var product = new Product($"Test Product {i}", $"Description {i}", 10.99m + i, 100);
            context.Products.Add(product);
        }

        // Create test customers
        for (int i = 1; i <= 5; i++)
        {
            var customer = new Customer($"Test Customer {i}", $"test{i}@example.com", $"123456789{i}");
            context.Customers.Add(customer);
        }

        await context.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }
} 