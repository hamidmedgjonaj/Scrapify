using Scrapify.ConsoleApp;
using Scrapify.Core.Services;
using System.Diagnostics;

static async Task RunApp()
{
    Stopwatch stopWatch = Stopwatch.StartNew();
    
    string url = "https://books.toscrape.com";
    string rootDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ScrapedWebsite");
    Directory.CreateDirectory(rootDirectory);

    var webScraper = new WebScraper(rootDirectory);

    var progressLogger = new ProgressLogger();
    webScraper.ProgressReport += progressLogger.OnProgressReported;

    await webScraper.ScrapWebsite(url);

    stopWatch.Stop();

    Console.WriteLine("Total scraping execution time: {0}ms", stopWatch.Elapsed.TotalMilliseconds);
}

await RunApp();