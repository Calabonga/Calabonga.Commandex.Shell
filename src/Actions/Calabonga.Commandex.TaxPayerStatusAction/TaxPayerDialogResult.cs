using System.Net.Http;
using System.Net.Http.Json;
using Calabonga.Commandex.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Calabonga.Commandex.TaxPayerStatusAction;

public partial class TaxPayerDialogResult : ViewModelBase, IDialogResult
{
    private readonly HttpClient _client;

    public TaxPayerDialogResult()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://statusnpd.nalog.ru");
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExecuteCheckCommand))]
    private string _value;

    [ObservableProperty]
    private NalogResponse? _nalogResponse;

    public string DialogTitle => "Проверка на nalog.ru";

    [RelayCommand(CanExecute = nameof(CanExecuteCheck))]
    private async Task ExecuteCheck()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/tracker/taxpayer_status", new
        {
            inn = Value,
            requestDate = DateTime.UtcNow.ToString("yyyy-MM-dd")
        });

        Value = string.Empty;

        NalogResponse = await response.Content.ReadFromJsonAsync<NalogResponse>();
    }


    private bool CanExecuteCheck => !string.IsNullOrEmpty(Value) && Value.Length is >= 10 and <= 12;

}