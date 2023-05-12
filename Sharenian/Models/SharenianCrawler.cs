using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using HtmlAgilityPack;

namespace Sharenian.Models;

public class SharenianCrawler
{
    private static string BaseUrl => "https://maplestory.nexon.com";

    private int ServerCode { get; }

    public SharenianCrawler(int serverCode) => ServerCode = serverCode;

    public async Task<List<Guild>> GetGuilds(int page)
    {
        var response = $"{BaseUrl}/N23Ranking/World/Guild?w={ServerCode}&t=2&page={page}".GetAsync().Result;
        var doc = await response.ResponseMessage.Content.ReadAsStringAsync();

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(doc);

        var rawData = htmlDoc.DocumentNode.InnerText.Split("\r\n")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select((x, i) => new { Idx = i, Str = x.Trim() }).ToList();

        var list = new List<Guild>();
        var startIdx = rawData.Find(x => x.Str.Equals("점수"))?.Idx;
        var endIdx = rawData.Find(x => x.Str.Equals("Family Site"))?.Idx - 10;

        if (startIdx != null && endIdx != null)
        {
            rawData = rawData.Skip(startIdx.Value + 1).Take(endIdx.Value - startIdx.Value - 1).ToList();

            for (var (i, order) = (page == 1 ? 1 : 2, 10 * (page - 1) + 1); i < rawData.Count; )
            {
                var (name, master) = (rawData[i].Str, rawData[i + 2].Str);
                var (level, score) = (ParseNumber(rawData[i + 1].Str) ?? 0, ParseNumber(rawData[i + 3].Str) ?? 0);

                list.Add(new Guild(order++, name, level, master, score));
                i += order < 4 ? 5 : 6;
            }
        }

        return list;
    }

    private static int? ParseNumber(string str)
    {
        var result = int.TryParse(str, NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var num);

        return result ? num : null;
    }
}