using Web.Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseCustomAuthentication();
app.MapGet("/", () => "Hello World!");
app.Run();
