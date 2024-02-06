using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Middleware 
{
    public class Authentication
    {
        private readonly RequestDelegate _next;

        public Authentication(RequestDelegate next) 
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) 
        {
            if(context.Request.Query["username"] == "user1" && context.Request.Query["password"] == "password1") 
            {
                context.Request.HttpContext.Items.Add("userdetails", "user1 password1");
                await _next(context);
            } 
            else 
            {
                await context.Response.WriteAsync("Failed!");
            }
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomAuthentication(this IApplicationBuilder builder) {
            return builder.UseMiddleware<Authentication>();
        }
    }
}
