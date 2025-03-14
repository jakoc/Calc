using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseWebRoot("wwwroot");

builder.WebHost.UseUrls("http://129.151.223.141");


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

// Tilføj services til DI-containeren
builder.Services.AddControllers();
builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();


var app = builder.Build();

app.UseCors("AllowAll");

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