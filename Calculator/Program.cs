using Calculator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Hent URL fra miljøvariabel eller appsettings.json
var appUrl = Environment.GetEnvironmentVariable("APP_BASE_URL") ?? builder.Configuration["Application:BaseUrl"];

if (string.IsNullOrWhiteSpace(appUrl))
{
    throw new InvalidOperationException("BaseUrl is not configured. Please set 'APP_BASE_URL' environment variable or 'Application:BaseUrl' in appsettings.json.");
}

Console.WriteLine($"Starter server på: {appUrl}");

builder.WebHost.UseUrls(appUrl);



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
app.MapGet("/", (context) =>
{
    context.Response.Redirect("/index.html");
    return Task.CompletedTask;
});

app.UseRouting();
app.MapControllers();


await app.RunAsync();

