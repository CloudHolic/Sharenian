using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sharenian.Api.MapleStory.Models;

public class GuildBasic
{
    public class GuildSkill
    {
        [JsonPropertyName("skill_name")]
        public required string Name { get; set; }

        [JsonPropertyName("skill_description")]
        public required string Description { get; set; }

        [JsonPropertyName("skill_level")]
        public long Level { get; set; }

        [JsonPropertyName("skill_effect")]
        public required string Effect { get; set; }

        [JsonPropertyName("skill_icon")]
        public required string Icon { get; set; }
    }

    [JsonPropertyName("date")]
    public required string Date { get; set; }

    [JsonPropertyName("world_name")]
    public required string World { get; set; }

    [JsonPropertyName("guild_name")]
    public required string Name { get; set; }

    [JsonPropertyName("guild_level")]
    public long Level { get; set; }

    [JsonPropertyName("guild_fame")]
    public long Fame { get; set; }

    [JsonPropertyName("guild_point")]
    public long Point { get; set; }

    [JsonPropertyName("guild_master_name")]
    public required string Master { get; set; }

    [JsonPropertyName("guild_member_count")]
    public required long MemberCount { get; set; }

    [JsonPropertyName("guild_member")]
    public required List<string> Members { get; set; }

    [JsonPropertyName("guild_skill")]
    public required List<GuildSkill> Skills { get; set; }

    [JsonPropertyName("guild_noblesse_skill")]
    public required List<GuildSkill> Noblesse { get; set; }

    [JsonPropertyName("guild_mark")]
    public required string Mark { get; set; }

    [JsonPropertyName("guild_mark_custom")]
    public required string CustomMark { get; set; }
}
