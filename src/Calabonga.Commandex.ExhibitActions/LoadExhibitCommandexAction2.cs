using System.Text.Json;
using Calabonga.Commandex.Contracts;

namespace Calabonga.Commandex.ExhibitActions;

public class LoadExhibitCommandexAction2 : ICommandexAction<Exhibit?>
{
    private readonly ExhibitRequest _request;
    private readonly HttpClient _client;

    public LoadExhibitCommandexAction2(ExhibitRequest request)
    {
        _request = request;
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://api.calabonga.com");
    }

    public async Task<Exhibit?> ExecuteAsync(CancellationToken cancellationToken)
    {
        if (!_request.IsActive)
        {
            return null;
        }

        var response = await _client.GetAsync("/api3/v3/random", cancellationToken);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<Exhibit>(content, JsonSerializerOptionsExt.Cyrillic);
    }

    public string Version => "v1.0.0-beta.1";
    public void ShowDialog()
    {
        throw new NotImplementedException();
    }

    public string TypeName => GetType().Name;

    public string DisplayName => "Получение экспоната из Музея Юмора";

    public string Description => "Запрос на удаленный API с целью получить экспонат одного из видов: анекдот, история, хокку, фразы и изречение, стишок и другие";
}