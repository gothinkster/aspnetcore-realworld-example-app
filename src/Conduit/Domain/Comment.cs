using System;
using System.Text.Json.Serialization;

namespace Conduit.Domain
{
    public class Comment
    {
        [JsonPropertyName("id")]
        public int CommentId { get; set; }

        public string? Body { get; set; }

        public Person? Author { get; set; }

        [JsonIgnore]
        public int AuthorId { get; set; }

        [JsonIgnore]
        public Article? Article { get; set; }

        [JsonIgnore]
        public int ArticleId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
