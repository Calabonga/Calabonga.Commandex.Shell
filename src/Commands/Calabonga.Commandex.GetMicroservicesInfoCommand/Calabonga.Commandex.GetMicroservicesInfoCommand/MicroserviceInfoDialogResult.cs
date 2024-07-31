using System.Collections.ObjectModel;
using System.Security;
using Calabonga.Commandex.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Calabonga.Commandex.GetMicroservicesInfoCommand;

public partial class GetMicroserviceInfoDialogResult : ViewModelBase, IDialogResult
{
    private readonly IDbConnectionFactory<NpgsqlConnection> _connectionFactory;
    private readonly IDialogService _dialogService;
    private readonly ILogger<GetMicroserviceInfoDialogResult> _logger;

    public GetMicroserviceInfoDialogResult(
        IDbConnectionFactory<NpgsqlConnection> connectionFactory,
        IDialogService dialogService,
        ILogger<GetMicroserviceInfoDialogResult> logger)
    {
        Title = "Список микросервисов";
        _connectionFactory = connectionFactory;
        _dialogService = dialogService;
        _logger = logger;
    }

    public string DialogTitle => Title;

    [NotifyPropertyChangedFor(nameof(IsValid))]
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoadMicroservicesCommand))]
    private string? _host = "localhost";

    [NotifyPropertyChangedFor(nameof(IsValid))]
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoadMicroservicesCommand))]
    private string? _database = "auth_openiddict";

    [NotifyPropertyChangedFor(nameof(IsValid))]
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoadMicroservicesCommand))]
    private string? _username = "qcrm_db_user";

    [NotifyPropertyChangedFor(nameof(IsValid))]
    [NotifyCanExecuteChangedFor(nameof(LoadMicroservicesCommand))]
    [ObservableProperty]
    private SecureString? _password;

    public bool IsValid => !string.IsNullOrEmpty(Host) &&
                           !string.IsNullOrEmpty(Database) &&
                           !string.IsNullOrEmpty(Username) &&
                           Password is not null;

    [ObservableProperty]
    private ObservableCollection<Microservice> _microservices = new();


    [RelayCommand(CanExecute = nameof(IsValid))]
    private async Task LoadMicroservices()
    {
        IsBusy = true;

        var password = new System.Net.NetworkCredential(string.Empty, Password).Password;

        var list = new List<Microservice>();

        await using var connection = _connectionFactory.CreateConnection($"User ID={Username};Password={password};Host={Host};Port=5432;Database={Database};");
        try
        {
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = "select \"Id\", \"Name\", \"Description\" from \"Microservices\"";

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Microservice()
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2)
                });
            }

            Microservices = new ObservableCollection<Microservice>(list);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            _dialogService.ShowError(exception.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }
}