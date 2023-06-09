﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using Microsoft.Win32;
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
    [UsedImplicitly]
    private async Task Load()
    {
        var progressHandler = new Progress<int>(value => Progress = value);
        var progress = progressHandler as IProgress<int>;
        Progress = 0;

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

        guilds.ForEach(x =>
        {
            x.SetPoint(guilds.Count);
            GuildList.Add(x);
        });
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    [UsedImplicitly]
    private async Task Export()
    {
        var saveFileDialog = new SaveFileDialog
        {
            InitialDirectory = Directory.GetCurrentDirectory(),
            Title = "저장 위치",
            DefaultExt = "xlsx",
            Filter = "xlsx files(*.xlsx)|*.xlsx"
        };

        if (!(saveFileDialog.ShowDialog() ?? false))
            return;

        var progressHandler = new Progress<int>(value => Progress = value);
        var manager = new ExcelManager(saveFileDialog.FileName);

        await manager.WriteExcel(GuildList.ToList(), progressHandler);

        _ = Process.Start("explorer.exe", $"/select, {saveFileDialog.FileName}");
    }

    #endregion
}
