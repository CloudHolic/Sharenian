using System.Text.Json.Serialization;

namespace Sharenian.Api.MapleStory.Models;

public class CharacterBasic
{
    [JsonPropertyName("date")]
    public required string Date { get; set; }

    [JsonPropertyName("character_name")]
    public required string Name { get; set; }

    [JsonPropertyName("world_name")]
    public required string World { get; set; }

    [JsonPropertyName("character_gender")]
    public required string Gender { get; set; }

    [JsonPropertyName("character_class")]
    public required string Class { get; set; }

    [JsonPropertyName("character_class_level")]
    public required string ClassLevel { get; set; }

    [JsonPropertyName("character_level")]
    public required long Level { get; set; }

    [JsonPropertyName("character_exp")]
    public required long Exp { get; set; }

    [JsonPropertyName("character_exp_rate")]
    public required string ExpRate { get; set; }

    [JsonPropertyName("character_guild_name")]
    public required string GuildName { get; set; }

    [JsonPropertyName("character_image")]
    public required string Image { get; set; }
}
