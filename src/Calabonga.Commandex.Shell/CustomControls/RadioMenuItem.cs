using System.Windows.Controls;

namespace Calabonga.Commandex.Shell.CustomControls;

public class RadioMenuItem : MenuItem
{
    public string GroupName { get; set; } = null!;

    protected override void OnClick()
    {
        if (Parent is ItemsControl itemsControl)
        {
            var menuItem = itemsControl.Items.OfType<RadioMenuItem>().FirstOrDefault(i => i.GroupName == GroupName && i.IsChecked);

            if (null != menuItem)
            {
                menuItem.IsChecked = false;
            }

            IsChecked = true;
        }
        base.OnClick();
    }
}