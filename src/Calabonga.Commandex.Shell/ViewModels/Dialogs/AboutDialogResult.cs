using Calabonga.Commandex.Engine.Dialogs;
using Calabonga.Commandex.Shell.Engine;
using Calabonga.Commandex.Shell.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Calabonga.Commandex.Shell.ViewModels.Dialogs;

public partial class AboutDialogResult : DefaultDialogResult
{
    private readonly IVersionService _versionService;
    private readonly FileService _fileService;
    private int Counter;

    public AboutDialogResult(IVersionService versionService, FileService fileService)
    {
        _versionService = versionService;
        _fileService = fileService;
        Title = "About Commandex";
    }

    public override WindowStyle WindowStyle => WindowStyle.None;

    [ObservableProperty]
    private string _version;

    [ObservableProperty]
    private string _branch;

    [ObservableProperty]
    private string _commit;

    [ObservableProperty]
    private string _tag;

    [ObservableProperty]
    private string _artifactsSize = "0.0 KB";

    [ObservableProperty]
    private string _artifactsFolder = "";

    [ObservableProperty]
    private string _commandsFolder = "";

    [ObservableProperty]
    private string _showSearchPanelOnStartup;

    [RelayCommand]
    private void CloseDialog() => ((Window)Owner!).Close();

    [RelayCommand]
    private void LoadData()
    {
        var total = ((float)_fileService.GetArtifactsSize() / 1024).ToString("F");
        ArtifactsSize = $"{total} KB";
        ArtifactsFolder = App.Current.Settings.ArtifactsFolderName;
        CommandsFolder = App.Current.Settings.CommandsPath;
        ShowSearchPanelOnStartup = App.Current.Settings.ShowSearchPanelOnStartup ? "Yes" : "No";
        Version = _versionService.Version;
        Branch = _versionService.Branch;
        Commit = _versionService.Commit;
        Tag = _versionService.Tag;
    }
}