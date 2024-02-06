using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Web.Middleware;
namespace Middleware.Tests;

public class AuthenticationTests
{
    [Fact]
    public async Task MiddlewareTest_FailWhenNotAuthenticated()
    {
        using var host = await new HostBuilder().ConfigureWebHost(webBuilder => {
            webBuilder.UseTestServer().ConfigureServices(services => {}).Configure(app => {
                app.UseMiddleware<Authentication>();
            });
        }).StartAsync();
        var response = await host.GetTestClient().GetAsync("/");
        Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Failed!", result);
    }
}
