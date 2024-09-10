﻿using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Commands;
using Calabonga.Commandex.Engine.Dialogs;
using Calabonga.Commandex.Shell.Engine;
using Calabonga.Commandex.Shell.Models;
using Calabonga.Commandex.Shell.ViewModels.Dialogs;
using Calabonga.Commandex.Shell.Views.Dialogs;
using Calabonga.PredicatesBuilder;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace Calabonga.Commandex.Shell.ViewModels;

public partial class ShellWindowViewModel : ViewModelBase
{
    private readonly CommandExecutor _commandExecutor;
    private readonly IConfigurationFinder _configurationFinder;
    private readonly IEnumerable<ICommandexCommand> _commands;
    private readonly ILogger<ShellWindowViewModel> _logger;
    private readonly IDialogService _dialogService;

    public ShellWindowViewModel(
        CommandExecutor commandExecutor,
        IConfigurationFinder configurationFinder,
        IEnumerable<ICommandexCommand> commands,
        ILogger<ShellWindowViewModel> logger,
        IDialogService dialogService)
    {
        Title = "CommandEx - Command Executor";
        _commandExecutor = commandExecutor;
        _configurationFinder = configurationFinder;
        _commands = commands;
        _logger = logger;
        _dialogService = dialogService;

        _commandExecutor.CommandPrepared += (_, _) => { IsBusy = false; };
        _commandExecutor.CommandPreparing += (_, _) => { IsBusy = true; };
    }

    [ObservableProperty]
    private string? _searchTerm;

    [ObservableProperty]
    private bool _isFindEnabled = App.Current.Settings.ShowSearchPanelOnStartup;

    [ObservableProperty]
    private ObservableCollection<CommandItem> _commandItems = new();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExecuteActionCommand))]
    [NotifyCanExecuteChangedFor(nameof(OpenCommandConfigurationCommand))]
    private CommandItem? _selectedCommand;

    private bool CanExecuteAction => SelectedCommand is not null;

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    private async Task ExecuteActionAsync()
    {
        var operation = await _commandExecutor.ExecuteAsync(SelectedCommand!);

        if (operation.Ok)
        {
            if (!operation.Result.IsPushToShellEnabled)
            {
                return;
            }

            var command = operation.Result;
            var message = CommandReport.CreateReport(command);
            _logger.LogInformation("{CommandType} executed with result: {Result}", command.TypeName, message);
            _dialogService.ShowNotification(message);
            return;
        }

        _logger.LogError(operation.Error, operation.Error.Message);
        _dialogService.ShowError(operation.Error.Message);
    }

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    private void OpenCommandConfiguration() => _configurationFinder.CommandConfiguration(SelectedCommand!.Scope);

    [RelayCommand]
    private void ShowAbout() => _dialogService.ShowDialog<AboutDialog, AboutDialogResult>();

    [RelayCommand]
    private void OpenLogsFolder()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

        if (!Path.Exists(path))
        {
            _dialogService.ShowNotification($"Folder not found: {path}");
            return;
        }

        Process.Start(new ProcessStartInfo
        {
            FileName = path,
            UseShellExecute = true,
            Verb = "open"
        });
    }

    [RelayCommand]
    private void LoadData()
    {
        IsBusy = true;

        FindCommands();

        IsBusy = false;
    }

    private void FindCommands()
    {
        var predicate = PredicateBuilder.True<ICommandexCommand>().And(x => !string.IsNullOrEmpty(x.Version));

        if (!string.IsNullOrEmpty(SearchTerm))
        {
            var term = SearchTerm.ToLower();
            predicate = predicate
                .And(x => x.DisplayName.ToLower().Contains(term))
                .Or(x => x.Description.ToLower().Contains(term))
                .Or(x => x.CopyrightInfo.ToLower().Contains(term))
                .Or(x => x.TypeName.ToLower().Contains(term))
                .Or(x => x.Version.ToLower().Contains(term));
        }

        var actionsList = _commands
            .Where(predicate.Compile())
            .Select(x => new CommandItem(x.GetType().Assembly.GetName().Name ?? "Commandex", x.TypeName, x.Version, x.DisplayName, x.Description))
            .ToList();

        CommandItems = new ObservableCollection<CommandItem>(actionsList);

        _logger.LogInformation("Total commands were found: {ActionCount}", actionsList.Count);
    }

    partial void OnSearchTermChanged(string? value) => FindCommands();

}