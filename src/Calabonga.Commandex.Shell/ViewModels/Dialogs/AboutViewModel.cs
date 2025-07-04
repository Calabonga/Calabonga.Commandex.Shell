﻿using Calabonga.Commandex.Engine.Dialogs;
using Calabonga.Commandex.Engine.Settings;
using Calabonga.Commandex.Shell.Engine;
using Calabonga.Commandex.Shell.Models;
using Calabonga.Commandex.Shell.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Calabonga.Commandex.Shell.ViewModels.Dialogs;

public sealed partial class AboutViewModel : DefaultDialogResult
{
    private readonly IDialogService _dialogService;
    private readonly ArtifactService _artifactService;
    private readonly FileService _fileService;
    private readonly CurrentAppSettings _currentAppSettings;

    public AboutViewModel(
        IAppSettings appSettings,
        IDialogService dialogService,
        ArtifactService artifactService,
        FileService fileService)
    {
        _dialogService = dialogService;
        _artifactService = artifactService;
        _fileService = fileService;
        Title = "About Commandex";
        _currentAppSettings = (CurrentAppSettings)appSettings;

        LoadData();
    }

    #region properties

    public override WindowStyle WindowStyle => WindowStyle.None;

    [ObservableProperty]
    private string _version;

    [ObservableProperty]
    private string _artifactsSize = "0.0 KB";

    [ObservableProperty]
    private string _artifactsFolder = "";

    [ObservableProperty]
    private string _commandsFolder = "";

    [ObservableProperty]
    private string _settingsFolder = "";

    [ObservableProperty]
    private string _showSearchPanelOnStartup;

    #endregion

    #region commands

    #region command CloseDialogCommand

    [RelayCommand]
    private void CloseDialog()
    {
        ((Window)Owner!).Close();
    }
    #endregion

    #region command ClearArtifactsCommand

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

    #endregion

    #region privates

    private void LoadData()
    {
        var total = ((float)_fileService.GetArtifactsSize() / 1024).ToString("F");
        ArtifactsSize = $"{total} KB";
        ArtifactsFolder = _currentAppSettings.ArtifactsFolderName;
        CommandsFolder = _currentAppSettings.CommandsPath;
        SettingsFolder = _currentAppSettings.SettingsPath;
        ShowSearchPanelOnStartup = _currentAppSettings.ShowSearchPanelOnStartup ? "Yes" : "No";
        Version = GetEngineVersion();
    }

    /// <summary>
    /// Extracts Engine version
    /// </summary>
    /// <returns></returns>
    private string GetEngineVersion()
    {
        var dll = AppDomain.CurrentDomain
            .GetAssemblies()
            .FirstOrDefault(x => x.GetName().Name?.Contains("Calabonga.Commandex.Engine") == true);

        if (dll is not null)
        {
            return dll.GetName().Version?.ToString() ?? "0.0.0.0";
        }

        return "0.0.0.0";
    }

    #endregion
}
