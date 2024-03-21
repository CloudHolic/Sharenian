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

        Point = (Order, percentage) switch
        {  
            (1, _) => 40,
            (2, _) => 38,
            (3, _) => 36,
            (4 or 5, _) => 34,
            (>= 6 and <= 10, _) => 32,
            (>= 11, < 6) => 29,
            (_, >= 6 and < 11) => 27,
            (_, >= 11 and < 16) => 25,
            (_, >= 16 and < 21) => 23,
            (_, >= 21 and < 26) => 21,
            (_, >= 26 and < 31) => 19,
            (_, >= 31 and < 41) => 17,
            (_, >= 41 and < 61) => 15,
            (_, >= 61 and < 81) => 13,
            _ when Score >= 500 => 10,
            _ => 0
        };
    }
}
