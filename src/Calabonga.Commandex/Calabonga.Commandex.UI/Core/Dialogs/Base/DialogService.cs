using System.Windows.Controls;
using System.Windows.Media;
using Calabonga.Commandex.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.Commandex.UI.Core.Dialogs.Base;

public class DialogService : IDialogService
{
    public void ShowDialog<TView, TViewModel>(Action<TViewModel> onClosingDialogCallback)
        where TView : IDialogView
        where TViewModel : IDialogResult
    {
        EventHandler closeEventHandler = null!;

        var dialog = new DialogWindow();

        var handler = closeEventHandler;
        closeEventHandler = (sender, _) =>
        {
            var window = (DialogWindow)sender!;
            var userControl = (UserControl)window.Content;
            var viewModel = ((TViewModel)userControl.DataContext);
            onClosingDialogCallback(viewModel);
            dialog.Closed -= handler;
        };

        dialog.Closed += closeEventHandler;

        var control = App.Current.Services.GetRequiredService(typeof(TView));
        var viewModel = App.Current.Services.GetRequiredService(typeof(TViewModel));
        ((UserControl)control!).DataContext = viewModel;
        dialog.Content = control;
        dialog.Title = ((IDialogResult)viewModel).DialogTitle;
        dialog.ShowDialog();
    }

    public void ShowNotification(string message)
    {
        ShowDialog(message, LogLevel.Notification);
    }

    public void ShowWarning(string message)
    {
        ShowDialog(message, LogLevel.Warning);
    }

    public void ShowError(string message)
    {
        ShowDialog(message, LogLevel.Error);
    }

    private Brush GetSolidColor(LogLevel type)
    {
        return type switch
        {
            LogLevel.Notification => new SolidColorBrush(Colors.Blue),
            LogLevel.Warning => new SolidColorBrush(Colors.DarkOrange),
            LogLevel.Error => new SolidColorBrush(Colors.Red),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private string GetTitle(LogLevel type)
    {
        return type switch
        {
            LogLevel.Notification => "Уведомление",
            LogLevel.Warning => "Предупреждение",
            LogLevel.Error => "Ошибка",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private void ShowDialog(string message, LogLevel type)
    {
        var dialog = new DialogWindow();

        EventHandler closeEventHandler = null!;

        var handler = closeEventHandler;
        closeEventHandler = (s, e) =>
        {
            dialog.Closed -= handler;
        };

        dialog.Closed += closeEventHandler;

        var control = new NotificationDialog
        {
            DataContext = new NotificationDialogViewModel
            {
                Title = message
            }
        };

        dialog.Content = control;

        dialog.Title = GetTitle(type);
        dialog.Foreground = GetSolidColor(type);
        dialog.Content = control;

        dialog.ShowDialog();
    }
}