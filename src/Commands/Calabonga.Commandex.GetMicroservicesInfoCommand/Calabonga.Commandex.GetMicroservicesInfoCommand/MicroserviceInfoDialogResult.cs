using System.Collections.ObjectModel;
using Calabonga.Commandex.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Calabonga.Commandex.GetMicroservicesInfoCommand;

public partial class GetMicroserviceInfoDialogResult : ViewModelBase, IDialogResult
{
    private readonly IDbConnectionFactory<NpgsqlConnection> _connectionFactory;
    private readonly ILogger<GetMicroserviceInfoDialogResult> _logger;

    public GetMicroserviceInfoDialogResult(
        IDbConnectionFactory<NpgsqlConnection> connectionFactory,
        ILogger<GetMicroserviceInfoDialogResult> logger)
    {
        Title = "Список микросервисов";
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public string DialogTitle => Title;

    [ObservableProperty]
    private ObservableCollection<Microservice> _microservices = new();

    [RelayCommand]
    private async Task LoadMicroservices()
    {
        IsBusy = true;

        var list = new List<Microservice>();

        await using var connection = _connectionFactory.CreateConnection("User ID=qcrm_db_user;Password=8jkGh47hnDw89Haq8LN2;Host=localhost;Port=5432;Database=auth_openiddict;");
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
        }
        finally
        {
            IsBusy = false;
        }
    }
}