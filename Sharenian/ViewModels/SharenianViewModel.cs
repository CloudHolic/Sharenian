using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Sharenian.Api.MapleStory;
using Sharenian.Models;
using Sharenian.Utils;

namespace Sharenian.ViewModels;

public partial class SharenianViewModel : ObservableRecipient
{
    #region Properties

    #region public ObservableCollection<Guild> GuildList

    private ObservableCollection<GuildInfo> _guildList = [];

    public ObservableCollection<GuildInfo> GuildList
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

    #region public ServerCode Server

    private ServerCode _server = ServerCode.Scania;

    public ServerCode Server
    {
        get => _server;
        set => SetProperty(ref _server, value);
    }

    #endregion

    #region public bool IsThisWeek

    private bool _isThisWeek;

    public bool IsThisWeek
    {
        get => _isThisWeek;
        set => SetProperty(ref _isThisWeek, value);
    }

    #endregion

    #endregion

    #region Commands

    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task Load()
    {
        var progressHandler = new Progress<int>(value => Progress = value);
        var progress = progressHandler as IProgress<int>;
        Progress = 0;

        var guilds = new List<GuildInfo>();
        for (var page = 1; ; page++)
        {
            var pagedGuild = await GuildApis.GetGuildRankingsAsync(Server.GetDescription(), page, IsThisWeek);
            if (pagedGuild.Count == 0)
                break;

            guilds.AddRange(pagedGuild);
            progress.Report(1000 * page / 12);
        }

        GuildList.Clear();
        guilds.ForEach(x =>
        {
            x.SetPoint(guilds.Count);
            GuildList.Add(x);
        });
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task Export()
    {
        var saveFileDialog = new SaveFileDialog
        {
            InitialDirectory = Directory.GetCurrentDirectory(),
            Title = "저장 위치",
            DefaultExt = "xlsx",
            Filter = "Xlsx files(*.xlsx)|*.xlsx"
        };

        if (!(saveFileDialog.ShowDialog() ?? false))
            return;

        var progressHandler = new Progress<int>(value => Progress = value);
        var manager = new ExcelManager(saveFileDialog.FileName);

        await manager.WriteGuildToExcel([.. GuildList], progressHandler);

        _ = Process.Start("explorer.exe", $"/select, {saveFileDialog.FileName}");
    }

    #endregion
}
