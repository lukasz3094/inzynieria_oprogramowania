namespace ApiTests.Helpers;

public class FakeHttpMessageHandler(HttpResponseMessage response) : HttpMessageHandler
{
	private readonly HttpResponseMessage _response = response;

	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return Task.FromResult(_response);
	}
}
