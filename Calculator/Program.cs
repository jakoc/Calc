using Calculator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Hent URL fra miljøvariabel eller appsettings.json
var appUrl = new ApplicationConfigurator(builder.Configuration).GetBaseUrl();  // Brug ApplicationConfigurator her

var databaseConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");

if (string.IsNullOrWhiteSpace(databaseConnectionString))
{
    throw new InvalidOperationException("Database connection string is not configured.");
}

Console.WriteLine($"Starter server på: {appUrl}");

builder.WebHost.UseUrls(appUrl);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://129.151.223.141") 
            .AllowAnyMethod()
            .AllowAnyHeader();

    });
});

builder.Services.AddControllers();
builder.Services.AddSingleton<DatabaseService>();

var app = builder.Build();
app.UseCors("AllowSpecificOrigin");

app.UseStaticFiles();
app.MapGet("/", (context) =>
{
    context.Response.Redirect("/index.html");
    return Task.CompletedTask;
});

app.UseRouting();
app.MapControllers();

await app.RunAsync();
