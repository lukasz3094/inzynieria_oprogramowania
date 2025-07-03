using Contracts.Singleton;
using Microsoft.Extensions.Configuration;

namespace Patterns.Singleton;

public class OutlookConfigManager : IOutlookConfigManager
{
    public string ClientId { get; }
    public string ClientSecret { get; }
    public string RedirectUri { get; }
    public string AuthEndpoint { get; }
    public string TokenEndpoint { get; }

    public OutlookConfigManager(IConfiguration configuration)
    {
        var section = configuration.GetSection("Outlook");
        ClientId = section["ClientId"]!;
        ClientSecret = section["ClientSecret"]!;
        RedirectUri = section["RedirectUri"]!;
        AuthEndpoint = section["AuthEndpoint"]!;
        TokenEndpoint = section["TokenEndpoint"]!;
    }
}
