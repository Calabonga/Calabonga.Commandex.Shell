using System.Net.Http;
using System.Text.Json;
using Calabonga.Commandex.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Calabonga.Commandex.QuizActions;

public partial class QuizDialogResult : ViewModelBase, IDialogResult
{
    private readonly HttpClient _client;
    public QuizDialogResult()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://quiz.calabonga.net/");
    }

    public string DialogTitle => "Quiz Title";

    public bool Ok => true;

    [ObservableProperty]
    private Question? _question;

    [RelayCommand]
    private async Task LoadQuestionAsync()
    {
        IsBusy = true;
        var cancellationTokenSource = new CancellationTokenSource();
        var response = await _client.GetAsync("/api/v1/questions/random", cancellationTokenSource.Token);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(cancellationTokenSource.Token);

        Question = JsonSerializer.Deserialize<Question>(content, JsonSerializerOptionsExt.Cyrillic);
        IsBusy = false;
    }
}