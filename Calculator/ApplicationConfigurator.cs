using Microsoft.Extensions.Configuration;

namespace Calculator;

public class ApplicationConfigurator
{
    private readonly IConfiguration _configuration;

    public ApplicationConfigurator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetBaseUrl()
    {
        var appUrl = Environment.GetEnvironmentVariable("APP_BASE_URL") ?? _configuration["Application:BaseUrl"];
        if (string.IsNullOrWhiteSpace(appUrl))
        {
            throw new InvalidOperationException("BaseUrl is not configured.");
        }
        return appUrl;
    }
}
