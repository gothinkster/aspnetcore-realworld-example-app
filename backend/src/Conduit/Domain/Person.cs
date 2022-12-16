using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Conduit.Domain
{
    public class Person
    {
        [JsonIgnore]
        public int PersonId { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? Bio { get; set; }

        public string? Image { get; set; }

        [JsonIgnore]
        public List<ArticleFavorite> ArticleFavorites { get; set; } = new();

        [JsonIgnore]
        public List<FollowedPeople> Following { get; set; } = new();

        [JsonIgnore]
        public List<FollowedPeople> Followers { get; set; } = new();

        [JsonIgnore]
        public byte[] Hash { get; set; } = Array.Empty<byte>();

        [JsonIgnore]
        public byte[] Salt { get; set; } = Array.Empty<byte>();
    }
}
