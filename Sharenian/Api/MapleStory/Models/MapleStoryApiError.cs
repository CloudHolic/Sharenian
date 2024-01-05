using System.Text.Json.Serialization;

namespace Sharenian.Api.MapleStory.Models;

public class MapleStoryApiError
{
    public class Error
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("message")]
        public required string Message { get; set; }
    }

    [JsonPropertyName("error")]
    public required Error ErrorMessage { get; set; }
}
