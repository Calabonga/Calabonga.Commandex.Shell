using System.Windows;
using Calabonga.Commandex.Engine;

namespace Calabonga.Commandex.Shell.Core.Dialogs;

/// <summary>
/// // Calabonga: Summary required (NotificationDialogResult 2024-07-29 04:07)
/// </summary>
public partial class NotificationDialogResult : DefaultDialogResult
{
    public override string DialogTitle => Title;

    /// <summary>
    /// Default value <see cref="WindowStyle.ToolWindow"/>
    /// </summary>
    public override WindowStyle WindowStyle => WindowStyle.ToolWindow;
}