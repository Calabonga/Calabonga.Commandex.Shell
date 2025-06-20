using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Shell.Infrastructure.Messaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace Calabonga.Commandex.Shell.ViewModels;

public partial class SearchModuleViewModel : ViewModelBase, IRecipient<ToggleFindVisibilityMessage>
{
    public SearchModuleViewModel()
    {
        WeakReferenceMessenger.Default.RegisterAll(this);
    }

    #region properties

    #region property IsFindEnabled
    [ObservableProperty]
    private bool _isFindEnabled = App.Current.Settings.ShowSearchPanelOnStartup;
    #endregion

    #region property SearchTerm
    [ObservableProperty]
    private string? _searchTerm;
    #endregion

    #endregion

    public void Receive(ToggleFindVisibilityMessage message)
    {
        IsFindEnabled = !IsFindEnabled;
    }


    partial void OnSearchTermChanged(string? value)
    {
        WeakReferenceMessenger.Default.Send(new SearchTermChangedMessage(value));
    }
}
