using System.Text.Json.Serialization;

namespace Sharenian.Api.MapleStory.Models;

public class Guild
{
    [JsonPropertyName("oguild_id")]
    public required string GuildId { get; set; }
}