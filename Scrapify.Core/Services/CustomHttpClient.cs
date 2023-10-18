using Scrapify.Core.Interfaces;

namespace Scrapify.Core.Services;

public class CustomHttpClient : IHttpClient
{
    private readonly HttpClient _httpClient;
    public CustomHttpClient()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> GetContent(string url, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    public async Task<Stream> GetResourceStream(string resourceUrl, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(resourceUrl, cancellationToken);
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }
}
