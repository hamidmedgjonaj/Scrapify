using Scrapify.Core.Parsers;

namespace Scrapy.UnitTests
{
    public class HtmlParserTests
    {
        private static readonly string Url = "https://www.w3schools.com";
        private static readonly string HtmlContent = """
                <!DOCTYPE html>
                <html>
                <body>
                    <a href="https://www.w3schools.com">This is a link</a>
                    <img src="w3schools.jpg" alt="W3Schools.com" width="104" height="142">
                </body>
                </html>
                """;

        [Fact]
        public void Parse_Should_Return_WebPage_With_HTMLContent()
        {
            // Arrange
            var htmlParser = new HtmlParser();

            // Act
            var webPage = htmlParser.Parse(Url, HtmlContent);

            // Assert
            Assert.Equal(webPage.Content, HtmlContent);
        }

        [Fact]
        public void Parse_Should_Return_Exact_Number_Of_Resources()
        {
            // Arrange
            var htmlParser = new HtmlParser();

            // Act
            var webPage = htmlParser.Parse(Url, HtmlContent);

            // Assert
            Assert.Equal(1, webPage.GetResources().Count);
        }
    }
}