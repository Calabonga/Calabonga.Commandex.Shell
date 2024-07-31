namespace Calabonga.Commandex.Contracts.Commands;

/// <summary>
/// // Calabonga: Summary required (ICommandexCommand 2024-07-31 07:55)
/// </summary>
public interface ICommandexCommand
{
    /// <summary>
    /// // Calabonga: Summary required (ICommandexCommand 2024-07-31 08:03)
    /// </summary>
    string CopyrightInfo { get; }

    /// <summary>
    /// // Calabonga: Summary required (ICommandexCommand 2024-07-31 08:03)
    /// </summary>
    string TypeName { get; }

    /// <summary>
    /// // Calabonga: Summary required (ICommandexCommand 2024-07-31 08:03)
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    /// // Calabonga: Summary required (ICommandexCommand 2024-07-31 08:03)
    /// </summary>
    string Description { get; }

    /// <summary>
    /// // Calabonga: Summary required (ICommandexCommand 2024-07-31 08:03)
    /// </summary>
    string Version { get; }

    /// <summary>
    /// // Calabonga: Summary required (ICommandexCommand 2024-07-31 08:03)
    /// </summary>
    Task ShowDialogAsync();

    /// <summary>
    /// // Calabonga: Summary required (ICommandexCommand 2024-07-31 08:03)
    /// </summary>
    bool IsPushToShellEnabled { get; }

    /// <summary>
    /// // Calabonga: Summary required (ICommandexCommand 2024-07-31 08:03)
    /// </summary>
    object? GetResult();
}