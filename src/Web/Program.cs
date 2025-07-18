using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Web.Services;
using CleanArchitecture.Web.Infrastructure;
using Microsoft.ApplicationInsights.Extensibility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddKeyVaultIfConfigured();
builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

// Add Application Insights
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddApplicationInsightsTelemetryProcessor<CustomTelemetryProcessor>();
builder.Services.AddSignalR();
builder.Services.AddScoped<ISignalRNotificationService, SignalRNotificationService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:Configuration"];
});
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Temporarily disabled database initialization due to migration conflicts
    // await app.InitialiseDatabaseAsync();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwaggerUi(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.UseExceptionHandler(options => { });
app.MapEndpoints();
app.MapHub<CleanArchitecture.Web.Services.InventoryHub>("/hubs/inventory");
app.MapHub<CleanArchitecture.Web.Services.OrderHub>("/hubs/orders");
app.UseCors();

app.Run();

public partial class Program { }