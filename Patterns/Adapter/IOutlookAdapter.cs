namespace Patterns.Adapter;

public interface IOutlookAdapter
{
    string GenerateAuthorizationUrl();
    Task<string> ExchangeCodeForTokenAsync(string code);
}
