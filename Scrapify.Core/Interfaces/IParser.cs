namespace Scrapify.Core.Interfaces;

public interface IParser<T>
{
    T Parse(string url, string content);
}
