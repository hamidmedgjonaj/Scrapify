using HtmlAgilityPack;
using Scrapify.Core.Interfaces;
using Scrapify.Core.Models;
using Scrapify.Core.Parsers;
using System.Net.Http;
using System.Threading;

namespace Scrapify.Core.Services;

public class WebScraper
{
    private readonly IHttpClient _httpClient;
    private readonly IParser<WebPage> _parser;
    private readonly string _rootDirectory;

    private static readonly ParallelOptions ParallelOptions = new() { MaxDegreeOfParallelism = 10 };

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

        foreach (var webPage in webPages)
        {
            foreach (var resource in webPage.GetResources())
            {
                await DownloadResource(resource, cancellationToken);
            }
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

            if (File.Exists(localResourceFilePath))
            {
                return;
            }

            if (!Directory.Exists(localResourceDirectory))
            {
                Directory.CreateDirectory(localResourceDirectory);
            }

            using var stream = await _httpClient.GetResourceStream(resource.Uri.AbsoluteUri, cancellationToken);            
            using var fileStream = new FileStream(localResourceFilePath, FileMode.Create);

            await stream.CopyToAsync(fileStream, cancellationToken);

            Console.WriteLine($"Downloaded resource: {localResourceFilePath}/{resource.FileName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to download resource: {resource.Uri.AbsoluteUri}. Error: {ex.Message}");
        }
    }
}
