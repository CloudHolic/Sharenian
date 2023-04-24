using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sharenian.Models;

namespace Sharenian.ViewModels;

public partial class MainWindowViewModel : ObservableRecipient
{
    #region Properties

    #region public ObservableCollection<Guild> GuildList
    
    private ObservableCollection<Guild> _guildList = null!;

    public ObservableCollection<Guild> GuildList
    {
        get => _guildList;
        set => SetProperty(ref _guildList, value);
    }

    #endregion

    #region public int Progress

    private int _progress;

    public int Progress
    {
        get => _progress;
        set => SetProperty(ref _progress, value);
    }

    #endregion

    #endregion

    public MainWindowViewModel() => GuildList = new ObservableCollection<Guild>();

    #region Commands

    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task Load()
    {
        var progressHandler = new Progress<int>(value => Progress = value);

        var progress = progressHandler as IProgress<int>;

        var guilds = await Task.Run(async () =>
        {
            var crawler = new SharenianCrawler(7);
            var result = new List<Guild>();
            for (var page = 1; ; page++)
            {
                var pagedGuild = await crawler.GetGuilds(page);
                if (pagedGuild.Count == 0)
                    break;

                result.AddRange(pagedGuild);
                progress.Report(100 * page / 150);
            }

            return result;
        });

        guilds.ForEach(GuildList.Add);
    }

    #endregion
}
