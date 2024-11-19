using CommunityToolkit.Mvvm.ComponentModel;

namespace Sharenian.Models;

public class GuildInfo(int order, string name, int level, string master, long score, int memberCount, int point = 0) : ObservableObject
{
    public int Order { get; set; } = order;

    public string Name { get; set; } = name;

    public int Level { get; set; } = level;

    public string Master { get; set; } = master;

    public long Score { get; set; } = score;

    public int MemberCount { get; set; } = memberCount;

    public int Point { get; set; } = point;

    public GuildInfo() : this(0, string.Empty, 1, string.Empty, 0, 0)
    {
    }

    public GuildInfo(GuildInfo prevGuild) : this(prevGuild.Order, prevGuild.Name, prevGuild.Level, prevGuild.Master, prevGuild.Score, prevGuild.MemberCount, prevGuild.Point)
    {
    }

    public void SetPoint(int totalGuilds)
    {
        var percentage = (double)Order / totalGuilds * 100;

        Point = (Order, Score, percentage) switch
        {  
            (1, _, _) => 40,
            (2, _, _) => 38,
            (3, _, _) => 36,
            (4 or 5, _, _) => 34,
            (>= 6 and <= 10, _, _) => 32,
            (>= 11, _, < 6) => 29,
            (_, _, >= 6 and < 11) => 27,
            var (_, s, p) when s >= 60_000 || p is >= 11 and < 16 => 25,
            var (_, s, p) when s >= 40_000 || p is >= 16 and < 21 => 23,
            var (_, s, p) when s >= 20_000 || p is >= 21 and < 26 => 21,
            var (_, s, p) when s >= 10_000 || p is >= 26 and < 31 => 19,
            var (_, s, p) when s >= 5_000 || p is >= 31 and < 41 => 17,
            var (_, s, p) when s >= 3_000 || p is >= 41 and < 61 => 15,
            var (_, s, p) when s >= 1_500 || p is >= 61 and < 81 => 13,
            (_, >= 500, _) => 10,
            _ => 0
        };
    }
}
