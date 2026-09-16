using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

// initialize a new instance of the webapplicationbuilder class with preconfigured defaults
var builder = WebApplication.CreateBuilder(args);


// add services for dependency injectino
builder.Services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy("host is up"), tags: ["live"]);

// builds the web application
var app = builder.Build();

// is the process up at all?
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = (check) => check.Tags.Contains("live")
});

// can this process serve real traffic?
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true
});

app.Run();