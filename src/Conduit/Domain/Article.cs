using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace Conduit.Domain
{
    public class Article
    {
        [JsonIgnore]
        public int ArticleId { get; set; }

        public string? Slug { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Body { get; set; }

        public Person? Author { get; set; }

        public List<Comment> Comments { get; set; } = new();

        [NotMapped]
        public bool Favorited => ArticleFavorites?.Any() ?? false;

        [NotMapped]
        public int FavoritesCount => ArticleFavorites?.Count ?? 0;

        [NotMapped]
        public List<string> TagList => ArticleTags.Where(x => x.TagId is not null).Select(x => x.TagId!).ToList();

        [JsonIgnore]
        public List<ArticleTag> ArticleTags { get; set; } = new();

        [JsonIgnore]
        public List<ArticleFavorite> ArticleFavorites { get; set; } = new();

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
