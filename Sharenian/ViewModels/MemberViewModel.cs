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

public partial class MemberViewModel : ObservableRecipient
{
    #region Properties

    #region public ObservableCollection<UserInfo> UserList

    private ObservableCollection<UserInfo> _userList = [];

    public ObservableCollection<UserInfo> UserList
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
    private async Task Load()
    {
        var progressHandler = new Progress<int>(value => Progress = value);
        var progress = progressHandler as IProgress<int>;
        Progress = 0;

        var members = await GuildApis.GetGuildMembersAsync(Guild, Server.GetDescription());
        progress.Report(1000 / (members.Count + 1));

        var users = new List<UserInfo>();
        for(var i = 0; i < members.Count; i++)
        {
            var nickname = members[i];
            var info = await CharacterApis.GetCharacterInfo(nickname);

            progress.Report(1000 * (i + 2) / (members.Count + 1));
            if (info.NickName != nickname)
                continue;

            users.Add(info);
        }

        UserList.Clear();
        users.ForEach(UserList.Add);
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

        await manager.WriteUserToExcel([.. UserList], progressHandler);

        _ = Process.Start("explorer.exe", $"/select, {saveFileDialog.FileName}");
    }

    #endregion
}
