using System.Windows;
using System.Windows.Controls;

namespace Calabonga.Commandex.Shell.CustomControls;

public class CommandItemListViewDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate DefaultTemplate { get; set; } = null!;

    public DataTemplate ExpandedTemplate { get; set; } = null!;

    public DataTemplate BriefTemplate { get; set; } = null!;



    /// <summary>When overridden in a derived class, returns a <see cref="T:System.Windows.DataTemplate" /> based on custom logic.</summary>
    /// <param name="item">The data object for which to select the template.</param>
    /// <param name="container">The data-bound object.</param>
    /// <returns>Returns a <see cref="T:System.Windows.DataTemplate" /> or <see langword="null" />. The default value is <see langword="null" />.</returns>
    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        return ExpandedTemplate;
    }
}