using System.Text;
using ApiTests.Helpers;
using Contracts.Singleton;
using FluentAssertions;
using Patterns.Adapter;
using Xunit;

namespace ApiTests;

public class OutlookAdapterTests
{
    private class FakeOutlookConfig : IOutlookConfigManager
    {
        public string ClientId => "client-id";
        public string ClientSecret => "client-secret";
        public string RedirectUri => "https://localhost";
        public string AuthEndpoint => "https://example.com/auth";
        public string TokenEndpoint => "https://example.com/token";
    }

    [Fact]
    public async Task ExchangeCodeForTokenAsync_ReturnsToken_WhenResponseIsValid()
    {
        var json = """{ "access_token": "test_token" }""";
        var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        var handler = new FakeHttpMessageHandler(response);
        var httpClient = new HttpClient(handler);
        var config = new FakeOutlookConfig();
        var adapter = new OutlookAdapter(config, httpClient);

        var token = await adapter.ExchangeCodeForTokenAsync("fake-code");

        token.Should().Be("test_token");
    }

    [Fact]
    public void GenerateAuthorizationUrl_ContainsExpectedParameters()
    {
        var config = new FakeOutlookConfig();
        var adapter = new OutlookAdapter(config, new HttpClient());

        var url = adapter.GenerateAuthorizationUrl();

        url.Should().Contain("client_id=client-id")
           .And.Contain("redirect_uri=https%3A%2F%2Flocalhost")
           .And.Contain("response_type=code");
    }
}
