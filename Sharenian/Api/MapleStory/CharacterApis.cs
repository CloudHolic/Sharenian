using System;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Sharenian.Api.MapleStory.Models;
using Sharenian.Models;
using Sharenian.Utils;

namespace Sharenian.Api.MapleStory;

public static class CharacterApis
{
    private static readonly string BaseAddress, Header, ApiKey;

    private static readonly Logger Logger;

    static CharacterApis()
    {
        var info = App.ApiInfos?.FirstOrDefault(x => x.Name == "MapleStory");
        BaseAddress = info?.BaseAddress ?? string.Empty;
        Header = info?.Header ?? string.Empty;
        ApiKey = info?.Key ?? string.Empty;

        Logger = Logger.Instance;
    }

    private static async Task<string> GetCharacterId(string characterName)
    {
        try
        {
            var result = await BaseAddress.AppendPathSegment("id")
                .SetQueryParam("character_name", characterName)
                .WithHeader(Header, ApiKey)
                .GetAsync()
                .ReceiveJson<Character>();

            return result.CharacterId;
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

    public static async Task<UserInfo> GetCharacterInfo(string characterName)
    {
        var ocId = await GetCharacterId(characterName);

        if (string.IsNullOrEmpty(ocId))
            return new UserInfo();

        try
        {
            var result = await BaseAddress.AppendPathSegments("character", "basic")
                .SetQueryParams(new
                {
                    ocid = ocId,
                    date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")
                }).WithHeader(Header, ApiKey)
                .GetAsync()
                .ReceiveJson<CharacterBasic>();

            return new UserInfo(result.Name, result.Class, result.Level, result.ExpRate);
        }
        catch (FlurlHttpException e)
        {
            var error = await e.GetResponseJsonAsync<MapleStoryApiError>();
            Logger.HttpError(error);
            return new UserInfo();
        }
        catch (Exception e)
        {
            Logger.Error(e);
            return new UserInfo();
        }
    }
}
