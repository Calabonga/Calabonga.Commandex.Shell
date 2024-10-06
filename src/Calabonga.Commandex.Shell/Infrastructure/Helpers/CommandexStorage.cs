﻿using Calabonga.Commandex.Shell.Infrastructure.Identity;
using Calabonga.Commandex.Shell.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Calabonga.Commandex.Shell.Infrastructure.Helpers;

public static class CommandexStorage
{
    /// <summary>
    /// Get user from storage
    /// </summary>
    /// <param name="settings"></param>
    public static void GetUser(CurrentAppSettings settings)
    {
        var data = FileHelper.GetData<CommandexData>(settings.CommandsPath);
        if (data is not null)
        {
            if (CheckTokenIsValid(data.Data.AccessToken!))
            {
                App.Current.SetUser(new ApplicationUser(data.Username, data.Data));
                return;
            }
            FileHelper.ClearData<CommandexData>(settings.CommandsPath);
        }
    }
    /// <summary>
    /// Save user to storage
    /// </summary>
    /// <param name="user"></param>
    /// <param name="settings"></param>
    public static void SetUser(ApplicationUser user, CurrentAppSettings settings)
    {
        var data = new CommandexData
        {
            Username = user.Name,
            Data = user.SecureData
        };
        FileHelper.SetData(data, settings.CommandsPath);
    }
    private static bool CheckTokenIsValid(string token)
    {
        var tokenTicks = GetTokenExpirationTime(token);
        var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;
        var now = DateTime.Now.ToUniversalTime();
        var valid = tokenDate >= now;
        return valid;
    }
    private static long GetTokenExpirationTime(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
        var ticks = long.Parse(tokenExp);
        return ticks;
    }
}
