using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Brainix.Host.Tests;

public sealed class HealthEndpointTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory = factory;

    [Fact]
    public async Task Readiness_endpoint_returns_200_and_reports_Healthy()
    {
        using var client = _factory.CreateClient();

        using var response = await client.GetAsync("/health");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Healthy", content);
    }

    [Fact]
    public async Task Liveness_endpoint_returns_200()
    {
        using var client = _factory.CreateClient();

        using var response = await client.GetAsync("/health/live");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Unknown_route_returns_404_not_a_stack_trace()
    {
        using var client = _factory.CreateClient();

        using var response = await client.GetAsync("/does-not-exist");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}