using CommunityToolkit.Mvvm.ComponentModel;

namespace Sharenian.Models;

public class User : ObservableObject
{
    public string NickName { get; set; }

    public string Job { get; set; }

    public int Level { get; set; }

    public int Murung { get; set; }

    public int LastActivity { get; set; }

    public User()
    {
        NickName = string.Empty;
        Job = string.Empty;
        Level = 0;
        Murung = 0;
        LastActivity = 0;
    }

    public User(string nickname, string job, int level, int murung, int lastActivity)
    {
        NickName = nickname;
        Job = job;
        Level = level;
        Murung = murung;
        LastActivity = lastActivity;
    }

    public User(User prevUser)
    {
        NickName = prevUser.NickName;
        Job = prevUser.Job;
        Level = prevUser.Level;
        Murung = prevUser.Murung;
        LastActivity = prevUser.LastActivity;
    }
}