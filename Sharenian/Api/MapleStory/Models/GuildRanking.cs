using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sharenian.Api.MapleStory.Models;

public class GuildRanking
{
    public class RankingInfo
    {
        [JsonPropertyName("date")]
        public required string Date { get; set; }

        [JsonPropertyName("ranking")]
        public required int Rank { get; set; }

        [JsonPropertyName("guild_name")]
        public required string Name { get; set; }

        [JsonPropertyName("world_name")]
        public required string World { get; set; }

        [JsonPropertyName("guild_level")]
        public required int Level { get; set; }

        [JsonPropertyName("guild_master_name")]
        public required string Master { get; set; }

        [JsonPropertyName("guild_mark")]
        public required string Mark { get; set; }

        [JsonPropertyName("guild_point")]
        public required long Point { get; set;}
    }

    [JsonPropertyName("ranking")]
    public required List<RankingInfo> Ranking { get; set; }
}
