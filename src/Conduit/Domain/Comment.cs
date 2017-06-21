using System;
using Newtonsoft.Json;

namespace Conduit.Domain
{
   public class Comment
    {
        [JsonProperty("id")]
        public int CommentId { get; set; }

        public string Body { get; set; }

        public Person Author { get; set; }

        [JsonIgnore]
        public Article Article { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}