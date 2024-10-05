using Calabonga.Commandex.Engine.Settings;
using Calabonga.Commandex.Shell.Infrastructure.Identity;
using Calabonga.Commandex.Shell.Infrastructure.Security;
using Calabonga.Commandex.Shell.Models;
using Calabonga.OperationResults;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace Calabonga.Commandex.Shell.Services;

/// <summary>
/// This is a IAuthenticationService implementation
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly CurrentAppSettings _currentAppSettings;
    private readonly IHttpClientFactory _httpClientFactory;
    public AuthenticationService(
        IAppSettings appSettings,
        IHttpClientFactory httpClientFactory)
    {
        _currentAppSettings = (CurrentAppSettings)appSettings;
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Returns the authenticated ApplicationUser if authentication on the server was successful
    /// or returns an error message about why authentication failed
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="clientType"></param>
    /// <returns></returns>
    public async Task<Operation<ApplicationUser, string>> AuthenticateUser(string username, string password, string clientType)
    {
        var url = _currentAppSettings.AuthorizationServerUrl;
        var clientId = _currentAppSettings.ClientId;
        var clientSecret = _currentAppSettings.ClientSecret;
        var grantType = _currentAppSettings.GrantType;

        if (string.IsNullOrEmpty(url)
            || string.IsNullOrEmpty(grantType)
            || string.IsNullOrEmpty(clientId)
            || string.IsNullOrEmpty(clientSecret))
        {
            return Operation.Error($"{nameof(App.Current.Settings.AuthorizationServerUrl)} is null. Authorization disabled.");
        }

        var content = new FormUrlEncodedContent(new Dictionary<string, string> {
            { "grant_type", grantType },
            { "username", username },
            { "password", password },
            { "client_id", clientId},
            { "client_secret", clientSecret },
            { "scope", "api" }
        });
        try
        {
            var serverUrl = url.EndsWith('/') ? $"{url}connect/token" : $"{url}/connect/token";
            using var client = _httpClientFactory.CreateClient(nameof(IAuthenticationService));
            var response = await client.PostAsync(serverUrl, content);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var responseText = await response.Content.ReadAsStringAsync();
                return Operation.Error(responseText);
            }
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var securityToken = JsonSerializer.Deserialize<SecureData>(result);
            if (securityToken is null)
            {
                return Operation.Error("Security token is null. Authentication failed");
            }
            var user = new ApplicationUser(username, securityToken);
            App.Current.SetUser(user);
            return Operation.Result(user);
        }
        catch (Exception exception)
        {
            return Operation.Error(exception.Message);
        }
    }
}
