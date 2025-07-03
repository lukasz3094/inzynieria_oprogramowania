using Contracts.Singleton;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Patterns.Adapter;

public class OutlookAdapter(IOutlookConfigManager config, HttpClient httpClient) : IOutlookAdapter
{
    private readonly IOutlookConfigManager _config = config;
    private readonly HttpClient _httpClient = httpClient;

	public string GenerateAuthorizationUrl()
	{
		return $"{_config.AuthEndpoint}?" +
			$"client_id={Uri.EscapeDataString(_config.ClientId)}&" +
			$"redirect_uri={Uri.EscapeDataString(_config.RedirectUri)}&" +
			$"response_type=code&" +
			$"scope={Uri.EscapeDataString("offline_access user.read calendars.readwrite")}";
	}

    public async Task<string> ExchangeCodeForTokenAsync(string code)
    {
        var parameters = new Dictionary<string, string>
        {
            { "client_id", _config.ClientId },
            { "client_secret", _config.ClientSecret },
            { "redirect_uri", _config.RedirectUri },
            { "code", code },
            { "grant_type", "authorization_code" }
        };

        var content = new FormUrlEncodedContent(parameters);
        var response = await _httpClient.PostAsync(_config.TokenEndpoint, content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("access_token").GetString()!;
    }
}
