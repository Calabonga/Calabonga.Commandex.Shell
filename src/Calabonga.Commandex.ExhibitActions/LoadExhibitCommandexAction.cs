using System.Text.Json;
using Calabonga.Commandex.Contracts;

namespace Calabonga.Commandex.ExhibitActions;

public class LoadExhibitCommandexAction : EmptyCommandexAction<Exhibit?>
{
    private readonly HttpClient _client = new();

    public LoadExhibitCommandexAction()
    {
        _client.BaseAddress = new Uri("https://api.calabonga.com");
    }

    public override string Version => "v1.0.0-beta.6";

    public override async Task ShowDialogAsync()
    {
        Result = await ExecuteAsync();
    }

    protected override Exhibit? Result { get; set; }

    public override string TypeName => GetType().Name;

    public override string DisplayName => "Получение экспоната из Музея Юмора";

    public override string Description => "Запрос на удаленный API с целью получить экспонат одного из видов: анекдот, история, хокку, фразы и изречение, стишок и другие";

    private async Task<Exhibit?> ExecuteAsync()
    {
        var response = await _client.GetAsync("/api3/v3/random");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Exhibit>(content, JsonSerializerOptionsExt.Cyrillic);
    }
}