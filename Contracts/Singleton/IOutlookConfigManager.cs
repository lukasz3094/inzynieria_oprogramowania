namespace Contracts.Singleton;

public interface IOutlookConfigManager
{
    string ClientId { get; }
    string ClientSecret { get; }
    string RedirectUri { get; }
    string AuthEndpoint { get; }
    string TokenEndpoint { get; }
}
