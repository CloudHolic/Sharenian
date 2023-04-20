using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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

    #region Commands

    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task Load()
    {

    }

    #endregion
}
