using System.Text.Json.Serialization;

namespace Conduit.Features.Profiles;

public class Profile
{
    public string? Username { get; init; }

    public string? Bio { get; init; }

    public string? Image { get; init; }

    [JsonPropertyName("following")]
    public bool IsFollowed { get; set; }
}
