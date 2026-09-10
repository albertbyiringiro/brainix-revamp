using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHealthChecks().AddCheck("self", static () => HealthCheckResult.Healthy("Host is up!"), tags: ["live"]);

var app = builder.Build();

// liveness: is the process up at all? (used by Kubernetes)
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = (check) => check.Tags.Contains("live"),
});

// readiness: is the process ready to serve traffic?
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
});

app.Run();

// Make the implicit Program class public so integration tests can access it
public partial class Program;