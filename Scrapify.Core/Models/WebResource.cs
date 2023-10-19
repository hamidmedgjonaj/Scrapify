namespace Scrapify.Core.Models;

public sealed class WebResource
{
    private readonly string _path = string.Empty;
    private readonly string _url = string.Empty;

    public Uri Uri { get; private set; }

    public string FileName { get; private set; }

    public bool IsExternalResource { get; private set; }

    public bool IsHTMLPage { get; private set; }

    public string Directory { get; private set; }

    public WebResource(string path, string url)
    {
        _path = path;
        _url = url;

        CheckIfExternalResource();

        Uri = GetResourceUri();
        FileName = ExtractFileNameFromUriAbsolutePath();
        Directory = GetDirectoryFromPath();
        IsHTMLPage = IsThisAnHTMLPage();
    }

    private Uri GetResourceUri()
    {
        if (!IsExternalResource)
        {
            return new Uri(new Uri(_url), _path);
        }
        return new Uri(_path);
    }

    private void CheckIfExternalResource()
    {
        if (_path.Contains("http") && !_path.Contains(_url))
        {
            IsExternalResource = true;
        }
    }

    private string ExtractFileNameFromUriAbsolutePath()
    { 
        return Path.GetFileName(Uri.AbsolutePath);
    }

    private string GetDirectoryFromPath()
    {
        string directory = IsExternalResource 
            ? "libs" 
            : Uri.AbsolutePath.Replace(FileName, "");

        return TrimFirstSlashCharacterFromStringIfAvailable(directory);
    }

    private bool IsThisAnHTMLPage()
    {
        return FileName.Contains(".html");
    }

    private static string TrimFirstSlashCharacterFromStringIfAvailable(string directory)
    {
        if (directory.Length > 0 && directory[0] == '/')
        {
            return directory.Substring(1);
        }

        return directory;
    }
}
