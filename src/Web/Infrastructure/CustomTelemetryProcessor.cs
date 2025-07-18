using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace CleanArchitecture.Web.Infrastructure;

public class CustomTelemetryProcessor : ITelemetryProcessor
{
    private readonly ITelemetryProcessor _next;

    public CustomTelemetryProcessor(ITelemetryProcessor next)
    {
        _next = next;
    }

    public void Process(ITelemetry item)
    {
        // Filter out health check requests
        if (item is RequestTelemetry requestTelemetry)
        {
            if (requestTelemetry.Url?.AbsolutePath == "/health")
            {
                return; // Don't send health check telemetry
            }

            // Add custom properties
            requestTelemetry.Properties["Environment"] = "Development";
            requestTelemetry.Properties["Application"] = "CleanArchitecture";
        }

        // Filter out dependency telemetry for Redis (too noisy)
        if (item is DependencyTelemetry dependencyTelemetry)
        {
            if (dependencyTelemetry.Type == "Redis" && dependencyTelemetry.Duration < TimeSpan.FromMilliseconds(10))
            {
                return; // Don't send fast Redis calls
            }
        }

        // Filter out trace telemetry below Information level
        if (item is TraceTelemetry traceTelemetry)
        {
            if (traceTelemetry.SeverityLevel < SeverityLevel.Information)
            {
                return; // Don't send debug/trace level telemetry
            }
        }

        _next.Process(item);
    }
} 