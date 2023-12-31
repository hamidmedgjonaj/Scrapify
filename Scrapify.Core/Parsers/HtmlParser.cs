﻿using HtmlAgilityPack;
using Scrapify.Core.Interfaces;
using Scrapify.Core.Models;

namespace Scrapify.Core.Parsers;

public sealed class HtmlParser : IParser<WebPage>
{
    public WebPage Parse(string url, string content)
    {
        var webPage = new WebPage(content);

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(content);

        foreach (var htmlNode in htmlDocument.DocumentNode.DescendantsAndSelf())
        {
            var resourcePath = htmlNode.GetAttributeValue("src", null)
                ?? htmlNode.GetAttributeValue("href", null);

            if (!string.IsNullOrEmpty(resourcePath))
            {
                webPage.AddResource(new WebResource(resourcePath, url));
            }
        }

        return webPage;
    }
}
