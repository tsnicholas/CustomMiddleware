using System;
using System.Collections.Generic;
using System.Linq;
using System.Thread.Tasks;

namespace Web.Middleware 
{
    public class Authentication 
    {
        private readonly RequestDelegate _next;

        public MyMiddleware(RequestDelegate next) 
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) 
        {
            if(context.Request.Query["username"] == "user1" && context.Request.Query["password"] == "password1") {
                await Context.Request.HttpContext.Items.Add("userdetails", "user1 password1");
                await _next(context);
            } else {
                await context.Request.WriteAsync("Failed!");
            }
        }
    }
}
