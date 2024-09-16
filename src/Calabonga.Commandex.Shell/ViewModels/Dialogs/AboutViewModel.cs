using Calabonga.Commandex.Engine.Dialogs;
using Calabonga.Commandex.Shell.Engine;
using Calabonga.Commandex.Shell.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Calabonga.Commandex.Shell.ViewModels.Dialogs;

public partial class AboutViewModel : DefaultViewModel
{
    private readonly IDialogService _dialogService;
    private readonly ILogger<AboutViewModel> _logger;
    private readonly IVersionService _versionService;
    private readonly FileService _fileService;

    public AboutViewModel(
        IDialogService dialogService,
        ILogger<AboutViewModel> logger,
        IVersionService versionService,
        FileService fileService)
    {
        _dialogService = dialogService;
        _logger = logger;
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
    }

    #region command ClearArtifacts

    private bool CanClearArtifacts => !string.IsNullOrEmpty(ArtifactsFolder);
    [RelayCommand(CanExecute = nameof(CanClearArtifacts))]
    private void ClearArtifacts()
    {
        IsBusy = true;
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ArtifactsFolder);

        if (!Path.Exists(path))
        {
            var message = $"{path} not exists";
            _logger.LogWarning(message);
            _dialogService.ShowWarning(message);
            IsBusy = false;
            return;
        }

        try
        {
            var directory = new DirectoryInfo(path);
            directory.Delete(true);
            LoadData();
        }
        catch (Exception exception)
        {
            var message = exception.Message;
            _logger.LogError(exception, message);

            Process.Start(new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true,
                Verb = "open"
            });
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion
}