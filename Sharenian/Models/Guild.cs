using CommunityToolkit.Mvvm.ComponentModel;

namespace Sharenian.Models;

public class Guild : ObservableObject
{
    public int Order { get; set; }

    public string Name { get; set; } 

    public int Level { get; set; }

    public string Master { get; set; }

    public int Score { get; set; }

    public Guild()
    {
        Order = 0;
        Name = string.Empty;
        Level = 1;
        Master = string.Empty;
        Score = 0;
    }

    public Guild(int order, string name, int level, string master, int score)
    {
        Order = order;
        Name = name;
        Level = level;
        Master = master;
        Score = score;
    }

    public Guild(Guild prevGuild)
    {
        Order = prevGuild.Order;
        Name = prevGuild.Name;
        Level = prevGuild.Level;
        Master = prevGuild.Master;
        Score = prevGuild.Score;
    }
}
