using System.Windows.Controls;

namespace Calabonga.Commandex.Shell.CustomControls;

public class RadioMenuItem : MenuItem
{
    public string GroupName { get; set; } = null!;

    protected override void OnClick()
    {
        if (Parent is ItemsControl ic)
        {
            var rmi = ic.Items.OfType<RadioMenuItem>().FirstOrDefault(i => i.GroupName == GroupName && i.IsChecked);

            if (null != rmi)
            {
                try
                {
                    rmi.IsChecked = false;

                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }

            IsChecked = true;
        }
        base.OnClick();
    }
}