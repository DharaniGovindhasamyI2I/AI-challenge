using Testcontainers.PostgreSql;
using Testcontainers.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Infrastructure.IntegrationTests.TestContainers;

public class TestContainersFixture : IAsyncDisposable
{
    public PostgreSqlContainer PostgreSqlContainer { get; }
    public RedisContainer RedisContainer { get; }
    public IHost Host { get; private set; } = null!;

    public TestContainersFixture()
    {
        PostgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15")
            .WithDatabase("testdb")
            .WithUsername("testuser")
            .WithPassword("testpass")
            .WithCleanUp(true)
            .Build();

        RedisContainer = new RedisBuilder()
            .WithImage("redis:7-alpine")
            .WithCleanUp(true)
            .Build();
    }

    public async Task InitializeAsync()
    {
        // Start containers
        await PostgreSqlContainer.StartAsync();
        await RedisContainer.StartAsync();

        // Create host with test configuration
        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Configure database
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(PostgreSqlContainer.GetConnectionString()));

                // Configure Redis
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = RedisContainer.GetConnectionString();
                    options.InstanceName = "Test_";
                });

                // Add other services as needed
                services.AddScoped<ICacheService, RedisCacheService>();
            })
            .Build();

        // Initialize database
        using var scope = Host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (Host != null)
        {
            await Host.DisposeAsync();
        }

        await PostgreSqlContainer.DisposeAsync();
        await RedisContainer.DisposeAsync();
    }
} 