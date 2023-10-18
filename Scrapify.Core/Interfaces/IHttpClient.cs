namespace Scrapify.Core.Interfaces;

public interface IHttpClient
{
    Task<string> GetContent(string url, CancellationToken cancellationToken = default);
    Task<Stream> GetResourceStream(string url, CancellationToken cancellationToken = default);
}
