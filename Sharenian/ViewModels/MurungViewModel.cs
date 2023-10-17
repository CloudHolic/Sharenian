using System;
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

public partial class MurungViewModel : ObservableRecipient
{
    #region Properties

    #region public ObservableCollection<Guild> GuildList

    private ObservableCollection<User> _userList = new();

    public ObservableCollection<User> UserList
    {
        get => _userList;
        set => SetProperty(ref _userList, value);
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

    #region public string Guild

    private string _guild = "아이엠캔들";

    public string Guild
    {
        get => _guild;
        set => SetProperty(ref _guild, value);
    }

    #endregion

    #endregion

    #region Commands

    [RelayCommand(AllowConcurrentExecutions = false)]
    [UsedImplicitly]
    private async Task Load()
    {
        var progressHandler = new Progress<int>(value => Progress = value);
        var progress = progressHandler as IProgress<int>;
        Progress = 0;

        var users = await Task.Run(() =>
        {
            var users = WebCrawler.GetGuildMembers(Server, Guild);
            progress.Report(1000 / (users.Count + 1));
            return users;
        }).ContinueWith(task =>
        {
            var users = task.Result;
            for (var i = 0; i < users.Count; i++)
            {
                users[i].Murung = WebCrawler.GetMurung(users[i].NickName);
                progress.Report(1000 * (i + 2) / (users.Count + 1));
            }

            return users;
        });

        UserList.Clear();
        users.ForEach(UserList.Add);
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

        await manager.WriteUserToExcel(UserList.ToList(), progressHandler);

        _ = Process.Start("explorer.exe", $"/select, {saveFileDialog.FileName}");
    }

    #endregion
}
