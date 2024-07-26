using System.Windows.Controls;
using System.Windows.Media;

namespace Calabonga.Commandex.UI.Core.Dialogs;

public class DialogService : IDialogService
{
    public void ShowDialog<TView, TViewModel>(Action<TViewModel> callback) where TView : IDialogView
    {
        var dialog = new DialogWindow();

        EventHandler closeEventHandler = null!;

        var handler = closeEventHandler;
        closeEventHandler = (s, e) =>
        {
            var view = ((DialogWindow)s!).Content as IDialogView;
            var viewModel = (TViewModel)view?.ViewModel!;
            callback(viewModel);
            dialog.Closed -= handler;
        };

        dialog.Closed += closeEventHandler;

        var viewModel = Activator.CreateInstance(typeof(TViewModel));
        var view = Activator.CreateInstance(typeof(TView));
        ((UserControl)view!).DataContext = viewModel;
        dialog.Content = view;

        dialog.ShowDialog();
    }

    public void ShowDialog(string message, LogLevel type)
    {
        var dialog = new DialogWindow();

        EventHandler closeEventHandler = null!;

        var handler = closeEventHandler;
        closeEventHandler = (s, e) =>
        {
            dialog.Closed -= handler;
        };

        dialog.Closed += closeEventHandler;

        var viewModel = new NotificationDialogViewModel { Title = message };
        var view = new NotificationDialog
        {
            DataContext = viewModel
        };

        dialog.Title = GetTitle(type);
        dialog.Foreground = GetSolidColor(type);
        dialog.Content = view;

        dialog.ShowDialog();
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
}