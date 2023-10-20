using Scrapify.Core.Interfaces;
using Scrapify.Core.Models;
using Scrapify.Core.Parsers;
using System.Data;

namespace Scrapify.Core.Services;

public class WebScraper
{
    private readonly IHttpClient _httpClient;
    private readonly IParser<WebPage> _parser;
    private readonly string _rootDirectory;

    private static readonly ParallelOptions ParallelOptions = new() { MaxDegreeOfParallelism = 10 };

    public event Action<WebScraper, double> ProgressReport;

    public WebScraper(string rootDirectory)
    {
        _rootDirectory = rootDirectory;
        _httpClient = new CustomHttpClient();
        _parser = new HtmlParser();
    }

    public async Task ScrapWebsite(string url, CancellationToken cancellationToken = default)
    {
        List<WebPage> webPages = new();

        WebPage mainPage = await ScrapeWebPage(url, cancellationToken);
        webPages.Add(mainPage);

        await ScrapChildPagesFromMainPage(mainPage, webPages, cancellationToken);

        double iteration = 0;
        double step = Math.Round(100 / (double)webPages.Count, 1, MidpointRounding.ToEven);

        foreach (var webPage in webPages)
        {
            await Parallel.ForEachAsync(
                webPage.GetResources(),
                ParallelOptions,
                async (resource, cancellationToken) =>
                {
                    await DownloadResource(resource, cancellationToken);
                });

            OnProgressReported((double)iteration / 100);
            iteration += step;
        }
    }

    private async Task ScrapChildPagesFromMainPage(
        WebPage mainPage, 
        List<WebPage> webPages, 
        CancellationToken cancellationToken = default)
    {
        List<string> pageUrls = mainPage.GetResources()
            .Where(p => p.IsHTMLPage)
            .Select(s => s.Uri.AbsoluteUri)
            .Distinct()
            .ToList();

        foreach (var pageUrl in pageUrls)
        {
            var webPage = await ScrapeWebPage(pageUrl, cancellationToken);
            webPages.Add(webPage);
        }
    }

    private async Task<WebPage> ScrapeWebPage(string url, CancellationToken cancellationToken = default)
    {
        var htmlContent = await _httpClient.GetContent(url, cancellationToken);

        return _parser.Parse(url, htmlContent);
    }

    private async Task DownloadResource(WebResource resource, CancellationToken cancellationToken)
    {
        try
        {
            string localResourceDirectory = Path.Combine(_rootDirectory, resource.Directory);
            string localResourceFilePath = Path.Combine(localResourceDirectory, resource.FileName);

            if (!File.Exists(localResourceFilePath))
            {
                if (!Directory.Exists(localResourceDirectory))
                {
                    Directory.CreateDirectory(localResourceDirectory);
                }

                using var stream = await _httpClient.GetResourceStream(resource.Uri.AbsoluteUri, cancellationToken);
                using var fileStream = new FileStream(localResourceFilePath, FileMode.Create);

                await stream.CopyToAsync(fileStream, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            if (ex is not IOException)
            {
                Console.WriteLine($"Failed to download resource: {resource.Uri.AbsoluteUri}. Error: {ex.Message}");
            }
        }
    }

    protected virtual void OnProgressReported(double progress)
    {
        ProgressReport?.Invoke(this, progress);
    }
}
