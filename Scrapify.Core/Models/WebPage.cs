namespace Scrapify.Core.Models;

public class WebPage
{
    private readonly List<WebResource> _resources = new();

    public string Content { get; private set; } = string.Empty;

    public WebPage(string content)
    {
        Content = content;
    }

    public IReadOnlyList<WebResource> GetResources() => _resources.ToList();

    internal void AddResource(WebResource resource)
    {
        _resources.Add(resource);
    }
}
