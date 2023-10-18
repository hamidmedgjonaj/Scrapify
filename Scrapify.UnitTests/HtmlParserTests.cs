using Scrapify.Core.Models;
using Scrapify.Core.Parsers;

namespace Scrapy.UnitTests
{
    public class HtmlParserTests
    {
        [Fact]
        public void Parse_Should_Return_WebPage_With_HTMLContent()
        {
            // Arrange
            string url = "https://www.w3schools.com";

            string htmlContent = """
                <!DOCTYPE html>
                <html>
                <body>
                    <a href="https://www.w3schools.com">This is a link</a>
                    <img src="w3schools.jpg" alt="W3Schools.com" width="104" height="142">
                </body>
                </html>
                """;

            var htmlParser = new HtmlParser();

            // Act
            var webPage = htmlParser.Parse(url, htmlContent);

            // Assert
            Assert.Equal(webPage.Content, htmlContent);
        }
    }
}