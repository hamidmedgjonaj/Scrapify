namespace Scrapify.Core.Models;

public class WebPage
{
    public string Content { get; set; } = string.Empty;

    public List<WebResource> Resources { get; set; } = new();
}
