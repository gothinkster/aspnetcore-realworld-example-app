
using System.Text.Json.Serialization;

namespace Conduit.IntegrationTests.Features.Articles
{
    public partial class ArticleTestData
    {
        [JsonPropertyName("article")]
        public ArticlePayload Article { get; set; }
    }

    public partial class ArticlePayload
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("tagList")]
        public string[] TagList { get; set; }
    }
}
