using System.Text.Json;
using Calabonga.Commandex.Contracts;
using Calabonga.Commandex.Contracts.Commands;

namespace Calabonga.Commandex.ExhibitCommand;

public class LoadExhibitCommand : EmptyCommandexCommand<Exhibit?>
{
    private readonly HttpClient _client = new();

    public LoadExhibitCommand()
    {
        _client.BaseAddress = new Uri("https://api.calabonga.com");
    }

    public override string Version => "v1.0.0-beta.8";

    public override async Task ShowDialogAsync()
    {
        Result = await ExecuteAsync();
    }

    public override string CopyrightInfo => "Calabonga SOFT © 2024";

    protected override Exhibit? Result { get; set; }

    public override string DisplayName => "Получение экспоната из Музея Юмора";

    public override string Description => "Запрос на удаленный API с целью получить экспонат одного из видов: анекдот, история, хокку, фразы и изречение, стишок и другие. Загруженные данные не отображаются.";

    private async Task<Exhibit?> ExecuteAsync()
    {
        var response = await _client.GetAsync("/api3/v3/random");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Exhibit>(content, JsonSerializerOptionsExt.Cyrillic);
    }
}