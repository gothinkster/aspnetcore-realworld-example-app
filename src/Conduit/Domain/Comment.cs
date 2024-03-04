using System;
using System.Text.Json.Serialization;

namespace Conduit.Domain;

public class Comment
{
    [JsonPropertyName("id")]
    public int CommentId { get; init; }

    public string? Body { get; init; }

    public Person? Author { get; init; }

    [JsonIgnore]
    public int AuthorId { get; init; }

    [JsonIgnore]
    public Article? Article { get; init; }

    [JsonIgnore]
    public int ArticleId { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}
