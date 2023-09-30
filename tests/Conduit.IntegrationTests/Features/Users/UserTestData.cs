
using System.Text.Json.Serialization;

namespace Conduit.IntegrationTests.Features.Users
{
    public partial class UserTestData
    {
        [JsonPropertyName("user")]
        public UserPayload User { get; set; }
    }

    public partial class UserPayload
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
