using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Sharenian.Api.MapleStory.Models;
using Sharenian.Models;
using Sharenian.Utils;

namespace Sharenian.Api.MapleStory;

public static class GuildApis
{
    private static readonly string BaseAddress, Header, ApiKey;

    private static readonly Logger Logger;

    static GuildApis()
    {
        var info = App.ApiInfos?.FirstOrDefault(x => x.Name == "MapleStory");
        BaseAddress = info?.BaseAddress ?? string.Empty;
        Header = info?.Header ?? string.Empty;
        ApiKey = info?.Key ?? string.Empty;

        Logger = Logger.Instance;
    }

    private static async Task<string> GetGuildIdAsync(string guildName, string worldName)
    {
        try
        {
            var result = await BaseAddress.AppendPathSegments("guild", "id")
                .SetQueryParams(new
                {
                    guild_name = guildName,
                    world_name = worldName
                }).WithHeader(Header, ApiKey)
                .GetAsync()
                .ReceiveJson<Guild>();

            return result.GuildId;
        }
        catch (FlurlHttpException e)
        {
            var error = await e.GetResponseJsonAsync<MapleStoryApiError>();
            Logger.HttpError(error);
            return string.Empty;
        }
        catch (Exception e)
        {
            Logger.Error(e);
            return string.Empty;
        }
    }

    public static async Task<List<string>> GetGuildMembersAsync(string guildName, string worldName)
    {
        var guildId = await GetGuildIdAsync(guildName, worldName);

        if (string.IsNullOrEmpty(guildId))
            return [];

        try
        {
            var result = await BaseAddress.AppendPathSegments("guild", "basic")
                .SetQueryParams(new
                {
                    oguild_id = guildId,
                    date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")
                }).WithHeader(Header, ApiKey)
                .GetAsync()
                .ReceiveJson<GuildBasic>();

            return result.Members;
        }
        catch (FlurlHttpException e)
        {
            var error = await e.GetResponseJsonAsync<MapleStoryApiError>();
            Logger.HttpError(error);
            return [];
        }
        catch (Exception e)
        {
            Logger.Error(e);
            return [];
        }
    }

    public static async Task<List<GuildInfo>> GetGuildRankingsAsync(string worldName, int page, bool thisWeek)
    {
        try
        {
            var date = thisWeek ? DateTime.Now : DateTime.Now.StartOfWeek(DayOfWeek.Monday);

            var result = await BaseAddress.AppendPathSegments("ranking", "guild")
                .SetQueryParams(new
                {
                    date = date.ToString("yyyy-MM-dd"),
                    world_name = worldName,
                    ranking_type = 2,
                    page
                }).WithHeader(Header, ApiKey)
                .GetAsync()
                .ReceiveJson<GuildRanking>();

            return result.Ranking
                .Select(x => new GuildInfo(x.Rank, x.Name, x.Level, x.Master, x.Point))
                .ToList();
        }
        catch (FlurlHttpException e)
        {
            var error = await e.GetResponseJsonAsync<MapleStoryApiError>();
            Logger.HttpError(error);
            return [];
        }
        catch (Exception e)
        {
            Logger.Error(e);
            return [];
        }
    }
}
