using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using Calabonga.Commandex.Contracts;

namespace Calabonga.Commandex.ExhibitActions;

public class LoadExhibitCommandexAction : ICommandexAction
{
    private readonly HttpClient _client;
    public LoadExhibitCommandexAction()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://api.calabonga.com/api3/v3");
    }

    public async Task ExecuteAsync()
    {
        var result = await _client.GetFromJsonAsync<Exhibit>("/random", new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

    }

    public string Name => GetType().Name;

    public string DisplayName => "Получение экспоната из Музея Юмора";

    public string Description => "Запрос на удаленный API с целью получить экспонат одного из видов: анекдот, история, хокку, фразы и изречение, стишок и другие";
}