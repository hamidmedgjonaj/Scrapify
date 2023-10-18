using HtmlAgilityPack;
using Scrapify.Core.Interfaces;
using Scrapify.Core.Models;

namespace Scrapify.Core.Parsers;

internal sealed class HtmlParser : IParser<WebPage>
{
    public WebPage Parse(string url, string content)
    {
        var webPage = new WebPage()
        {
            Content = content
        };

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(content);

        foreach(var htmlNode in htmlDocument.DocumentNode.DescendantsAndSelf())
        {
            var resourceName = htmlNode.GetAttributeValue("src", null) 
                ?? htmlNode.GetAttributeValue("href", null);

            if (IsValidResource(resourceName))
            {
                var resourceUrl = new Uri(new Uri(url), resourceName).AbsoluteUri;
                webPage.Resources.Add(new WebResource(resourceName, resourceUrl));
            }
        }

        return webPage;
    }

    private static bool IsValidResource(string? resourceName)
    {
        return !string.IsNullOrEmpty(resourceName)
            && !resourceName.Contains("http://")
            && !resourceName.Contains("https://");
    }
}
