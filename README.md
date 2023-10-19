# Scrapify - A Web Scraping Console Application in C#

This is a simple console application in C# that demonstrates web scraping using the HtmlAgilityPack library. It scrapes content from a specified URL and downloads associated resources like images, CSS files, and JavaScript files.

## How to Install

To run this application, you'll need:

- .NET Core SDK installed on your machine. You can download it from [here](https://dotnet.microsoft.com/download).
- Visual Studio or any text editor of your choice.

## How to Use

1. Clone or download this repository.

2. Open the project in Visual Studio or your preferred text editor.

3. Build the project.

4. Run the application.

The application will scrape the content of a predefined URL (e.g., "https://example.com"), extract the content, and download any associated resources like images, CSS files, and JavaScript files.

## Libraries Used

- **HtmlAgilityPack**: Used for parsing HTML content.

## Structure

The project follows SOLID principles and has the following structure:

- `Models`: Contains the `WebPage` class to hold scraped page data and `WebResource` class to hold resources.
- `Services`: Contains the `WebScraper` and `CustomHttpClient` classes for web scraping and making HTTP requests.
- `Parsers`: Contains the `HtmlParser` class to parse HTML content.
- `Interfaces`: Contains the `IHttpClient` and `IParser` interfaces.
- `Program.cs`: Entry point for the console application.

## Contributing

If you have any suggestions or found a bug, feel free to open an issue or create a pull request.