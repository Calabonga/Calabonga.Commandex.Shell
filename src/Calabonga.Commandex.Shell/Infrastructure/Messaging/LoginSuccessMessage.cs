using CommunityToolkit.Mvvm.Messaging;

namespace Calabonga.Commandex.Shell.Infrastructure.Messaging;

/// <summary>
/// User success login message. <see cref="WeakReferenceMessenger"/>
/// </summary>
/// <param name="Username"></param>
public sealed record LoginSuccessMessage(string Username);