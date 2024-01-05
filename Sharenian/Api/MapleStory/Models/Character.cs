using System.Text.Json.Serialization;

namespace Sharenian.Api.MapleStory.Models;

public class Character
{
    [JsonPropertyName("ocid")]
    public required string CharacterId { get; set; }
}
