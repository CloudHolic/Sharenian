using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;
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

    #region Public DateRange SelectedDate

    private DateRange _selectedDate = new();

    public DateRange SelectedDate
    {
        get => _selectedDate;
        set => SetProperty(ref _selectedDate, value);
    }

    #endregion

    #region Public ObservableCollection<DateRange> Dates

    private ObservableCollection<DateRange> _dates = [];

    public ObservableCollection<DateRange> Dates
    {
        get => _dates;
        set => SetProperty(ref _dates, value);
    }

    #endregion

    #endregion

    #region Constructor

    public SharenianViewModel()
    {
        var today = DateTime.Today;
        if (today.DayOfWeek == DayOfWeek.Thursday)
        {
            Dates.Add(new DateRange(today));
            Dates.Add(new DateRange(today.AddDays(-7)));
            SelectedDate = Dates[0];
        }
        else
        {
            var thursday = today.StartOfWeek(DayOfWeek.Thursday);
            Dates.Add(new DateRange(today));
            Dates.Add(new DateRange(thursday));
            Dates.Add(new DateRange(thursday.AddDays(-7)));
            SelectedDate = Dates[1];
        }
    }

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
            var pagedGuild = await GuildApis.GetGuildRankingsAsync(Server.GetDescription(), page, SelectedDate.PivotDate);
            if (pagedGuild.Count == 0)
                break;

            guilds.AddRange(pagedGuild);
            progress.Report(1000 * page / 12);
            Thread.Sleep(TimeSpan.FromSeconds(1));
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
