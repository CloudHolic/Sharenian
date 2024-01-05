using CommunityToolkit.Mvvm.ComponentModel;

namespace Sharenian.Models;

public class UserInfo(string nickname, string job, long level, string expRate) : ObservableObject
{
    public string NickName { get; set; } = nickname;

    public string Job { get; set; } = job;

    public long Level { get; set; } = level;

    public string ExpRate { get; set; } = expRate;

    public UserInfo() : this(string.Empty, string.Empty, 0, string.Empty)
    {
    }

    public UserInfo(UserInfo prevUser) : this(prevUser.NickName, prevUser.Job, prevUser.Level, prevUser.ExpRate)
    {
    }
}