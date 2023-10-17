using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Flurl.Http;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Sharenian.Models;

public static class WebCrawler
{
    private static string NexonBaseUrl => "https://maplestory.nexon.com";

    private static string MapleggBaseUrl => "https://maple.gg";
    
    public static async Task<List<Guild>> GetGuilds(ServerCode server, int page)
    {
        var response = $"{NexonBaseUrl}/N23Ranking/World/Guild?w={(int)server}&t=2&page={page}".GetAsync().Result;
        var doc = await response.ResponseMessage.Content.ReadAsStringAsync();

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(doc);

        var rawData = htmlDoc.DocumentNode.InnerText.Split("\r\n")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select((x, i) => new { Idx = i, Str = x.Trim() }).ToList();

        var list = new List<Guild>();
        var startIdx = rawData.Find(x => x.Str.Equals("점수"))?.Idx;
        var endIdx = rawData.Find(x => x.Str.Equals("Family Site"))?.Idx - 10;

        if (startIdx == null || endIdx == null) 
            return list;

        rawData = rawData.Skip(startIdx.Value + 1).Take(endIdx.Value - startIdx.Value - 1).ToList();

        for (var (i, order) = (page == 1 ? 1 : 2, 10 * (page - 1) + 1); i < rawData.Count; )
        {
            var (name, master) = (rawData[i].Str, rawData[i + 2].Str);
            var (level, score) = (ParseNumber(rawData[i + 1].Str) ?? 0, ParseNumber(rawData[i + 3].Str) ?? 0);

            list.Add(new Guild(order++, name, level, master, score));
            i += order < 4 ? 5 : 6;
        }

        return list;
    }

    public static async Task<List<User>> GetGuildMembers(ServerCode server, string guildName)
    {
        var html = await GetRawGuildMembers(server, guildName);

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var rawData = htmlDoc.DocumentNode.InnerText.Split("\r\n")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim()).ToList();

        var list = new List<User>();
        var startIdx = rawData.FindIndex(x => x.Equals("마스터"));
        var endIdx = rawData.FindIndex(x => x.Equals("MAPLE.GG"));

        if (startIdx < 0 || endIdx < 0)
            return list;

        rawData = rawData.Skip(startIdx + 1).Take(endIdx - startIdx - 1).ToList();
        
        for (var i = 0; i < rawData.Count; i++)
        {
            if (rawData[i].StartsWith("마지막 활동일"))
            {
                var jobAndLevel = rawData[i - 1].Split('/');
                var nickname = rawData[i - 2];
                var (job, level) = (jobAndLevel[0], int.Parse(jobAndLevel[1].Split('.')[1]));
                var lastActivity = rawData[i].Split(':')[1].Trim();
                list.Add(new User(nickname, job, level, 0, lastActivity));
            }
        }

        return list;
    }

    public static async Task<int> GetMurung(string userName)
    {
        var response = $"{MapleggBaseUrl}/u/{HttpUtility.UrlEncode(userName).ToUpper()}".GetAsync().Result;
        var doc = await response.ResponseMessage.Content.ReadAsStringAsync();

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(doc);

        var node = htmlDoc.DocumentNode.SelectSingleNode(
            "//div[@class='card border-bottom-0']/div/section" +
            "/div[@class='row text-center']/div[1]/section/div/div");

        var success = int.TryParse(node.InnerText.Trim().Split('\n')[0], out var murung);
        return success ? murung : 0;
    }

    private static async Task<string> GetRawGuildMembers(ServerCode server, string guildName)
    {
        var serverName = server.ToString().ToLower();
        var syncXpath = "//button[@id='btn-sync']";
        var loadXpath = "//*[@id='guild-content']/section/div[1]/div[1]/section/div[2]/div/div[1]/b/a";

        try
        {
            var options = new ChromeOptions();
            options.AddArgument("headless");

            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            using var driver = new ChromeDriver(service, options);
            driver.Url = $"{MapleggBaseUrl}/guild/{serverName}/{HttpUtility.UrlEncode(guildName).ToUpper()}";
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            var element = driver.FindElement(By.XPath(syncXpath));
            element.Click();

            var alert = driver.SwitchTo().Alert();
            alert.Accept();

            await Task.Delay(5000);

            // 제대로 로드되었나 확인
            _ = driver.FindElement(By.XPath(loadXpath));

            return driver.PageSource;
        }
        catch
        {
            return string.Empty;
        }
    }

    private static int? ParseNumber(string str)
    {
        var result = int.TryParse(str, NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var num);

        return result ? num : null;
    }
}