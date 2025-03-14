using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Tilføj services til DI-containeren
builder.Services.AddControllers();
builder.Services.AddSingleton<DatabaseService>();

var app = builder.Build();

// Gør det muligt at servere HTML, CSS og JS fra wwwroot/
app.UseStaticFiles();

// Omdiriger root "/" til index.html
app.MapGet("/", async (context) =>
{
    context.Response.Redirect("/index.html");
});

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();