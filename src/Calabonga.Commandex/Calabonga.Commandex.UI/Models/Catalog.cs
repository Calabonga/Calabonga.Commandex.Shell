namespace Calabonga.Commandex.UI.Models;

public class TreeItem : ItemBase
{

}

public class GroupTreeItem : ItemBase
{
    public TreeItem? TreeItem { get; set; }

    public List<TreeItem> Items { get; set; } = new();
}

public class ItemBase
{
    public string Name { get; set; }
}