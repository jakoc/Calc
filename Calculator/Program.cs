using Calculator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Hent URL fra miljøvariabel eller appsettings.json
var appUrl = new ApplicationConfigurator(builder.Configuration).GetBaseUrl();  // Brug ApplicationConfigurator her

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

builder.Services.AddControllers();
builder.Services.AddSingleton<DatabaseService>();

var app = builder.Build();
app.UseCors("AllowAll");

app.UseStaticFiles();
app.MapGet("/", (context) =>
{
    context.Response.Redirect("/index.html");
    return Task.CompletedTask;
});

app.UseRouting();
app.MapControllers();

await app.RunAsync();
