using Calabonga.Commandex.Engine.Dialogs;
using Calabonga.Commandex.Engine.Settings;
using Calabonga.Commandex.Shell.Engine;
using Calabonga.Commandex.Shell.Models;
using Calabonga.Commandex.Shell.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace Calabonga.Commandex.Shell.ViewModels.Dialogs;

public sealed partial class AboutViewModel : DefaultViewModel
{
    private readonly IDialogService _dialogService;
    private readonly ILogger<AboutViewModel> _logger;
    private readonly IVersionService _versionService;
    private readonly ArtifactService _artifactService;
    private readonly FileService _fileService;
    private readonly CurrentAppSettings _currentAppSettings;

    public AboutViewModel(
        IAppSettings appSettings,
        IDialogService dialogService,
        ILogger<AboutViewModel> logger,
        IVersionService versionService,
        ArtifactService artifactService,
        FileService fileService)
    {
        _dialogService = dialogService;
        _logger = logger;
        _versionService = versionService;
        _artifactService = artifactService;
        _fileService = fileService;
        Title = "About Commandex";
        _currentAppSettings = (CurrentAppSettings)appSettings;

        LoadData();
    }

    public override WindowStyle WindowStyle => WindowStyle.None;

    [ObservableProperty]
    private string _version;

    [ObservableProperty]
    private string _branch;

    [ObservableProperty]
    private string _commit;

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
        ArtifactsFolder = _currentAppSettings.ArtifactsFolderName;
        CommandsFolder = _currentAppSettings.CommandsPath;
        ShowSearchPanelOnStartup = _currentAppSettings.ShowSearchPanelOnStartup ? "Yes" : "No";
        Version = _versionService.Version;
        Branch = _versionService.Branch;
        Commit = _versionService.Commit;
    }

    #region command ClearArtifacts

    private bool CanClearArtifacts => !string.IsNullOrEmpty(ArtifactsFolder);
    [RelayCommand(CanExecute = nameof(CanClearArtifacts))]
    private void ClearArtifacts()
    {
        IsBusy = true;
        var result = _artifactService.CleanArtifactsFolder();

        if (!result.Ok)
        {
            _dialogService.ShowWarning(result.Error);
            IsBusy = false;
            return;
        }

        IsBusy = false;
    }

    #endregion
}