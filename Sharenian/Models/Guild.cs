using CommunityToolkit.Mvvm.ComponentModel;

namespace Sharenian.Models;

public class Guild : ObservableObject
{
    public int Order { get; set; }

    public string Name { get; set; } 

    public int Level { get; set; }

    public string Master { get; set; }

    public int Score { get; set; }

    public int Point { get; set; }

    public Guild()
    {
        Order = 0;
        Name = string.Empty;
        Level = 1;
        Master = string.Empty;
        Score = Point = 0;
    }

    public Guild(int order, string name, int level, string master, int score, int point = 0)
    {
        Order = order;
        Name = name;
        Level = level;
        Master = master;
        Score = score;
        Point = point;
    }

    public Guild(Guild prevGuild)
    {
        Order = prevGuild.Order;
        Name = prevGuild.Name;
        Level = prevGuild.Level;
        Master = prevGuild.Master;
        Score = prevGuild.Score;
        Point = prevGuild.Point;
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
