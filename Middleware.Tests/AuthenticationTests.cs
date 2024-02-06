using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Web.Middleware;
namespace Middleware.Tests;

public class AuthenticationTests : IAsyncLifetime
{
    private IHost? host;
    
    public Task DisposeAsync() 
    {
        return Task.CompletedTask;
    }

    public async Task InitializeAsync() 
    {
        host = await new HostBuilder().ConfigureWebHost(webBuilder => {
            webBuilder.UseTestServer().ConfigureServices(services => {}).Configure(app => {
                app.UseMiddleware<Authentication>();
                app.Run(async context => {
                    await context.Response.WriteAsync("Authenticated!");
                });
            });
        }).StartAsync();
    }

    [Fact]
    public async Task MiddlewareTest_FailWhenNotAuthenticated()
    {
        if(host == null) {
            throw new Exception("host is null.");
        }
        var response = await host.GetTestClient().GetAsync("/");
        Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Failed!", result);
    }

    [Fact]
    public async Task MiddlewareTest_FailWhenNoPassword() 
    {
        if(host == null) {
            throw new Exception("host is null.");
        }
        var response = await host.GetTestClient().GetAsync("/?username=user1");
        Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Failed!", result); 
    }

    [Fact]
    public async Task MiddlewareTest_FailWhenWrongPassword()
    {
        if(host == null) {
            throw new Exception("host is null.");
        }
        var response = await host.GetTestClient().GetAsync("/?username=user1&password=wrongPassword");
        Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Failed!", result);
    }

    [Fact]
    public async Task MiddlewareTest_Authenticated() {
        if(host == null) {
            throw new Exception("host is null.");
        }
        var response = await host.GetTestClient().GetAsync("/?username=user1&password=password1");
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Authenticated!", result);
    }
}
