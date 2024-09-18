using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Shell.Engine;
using Calabonga.Commandex.Shell.Tests.Commands;
using Moq;

namespace Calabonga.Commandex.Shell.Tests;

public class CommandFinderConverterTests
{
    [Fact]
    public void CommandFinder_CanFoundCommands()
    {
        var commands = GetCommands();
        Assert.NotEmpty(commands);
    }

    [Fact]
    public void CommandFinder_CanConvert_ToList()
    {
        const int expected = 6;

        var commands = GetCommands();
        var reader = new Mock<ISettingsReaderConfiguration>();
        var items = CommandFinder.ConvertToItems(commands, reader.Object, string.Empty);
        var actual = items.Count;

        Assert.NotEmpty(items);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CommandFinder_CanConvert_ToHierarchy_WithTotalItems_5()
    {
        const int expected = 5;

        var commands = GetCommands();
        var reader = new Mock<ISettingsReaderConfiguration>();
        var items = CommandFinder.ConvertToGroupedItems(new TestGroupBuilder(), commands, reader.Object, string.Empty).ToList();
        var actual = items.Count;

        Assert.NotEmpty(items);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CommandFinder_CanConvert_ToHierarchy_WithUntagged_Equals_1()
    {
        const int expected = 1;

        var commands = GetCommands();
        var reader = new Mock<ISettingsReaderConfiguration>();
        var items = CommandFinder.ConvertToGroupedItems(new TestGroupBuilder(), commands, reader.Object, string.Empty).ToList();
        var actual = items.Count(x => x.Name == "Untagged");

        Assert.NotEmpty(items);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(3, "One")]
    [InlineData(2, "Two")]
    [InlineData(1, "Three")]
    [InlineData(4, "Four")]
    public void CommandFinderShould_PutInGroup_CommandsTagged_ItemsCount(int expected, string groupName)
    {
        var commands = GetCommands();
        var reader = new Mock<ISettingsReaderConfiguration>();
        var items = CommandFinder.ConvertToGroupedItems(new TestGroupBuilder(), commands, reader.Object, string.Empty).ToList();
        var actual = items.Where(x => x.Name == groupName).SelectMany(x => x.Items).Count();

        Assert.NotEmpty(items);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(1, "Three")]
    [InlineData(3, "One")]
    [InlineData(2, "Two")]
    [InlineData(0, "Four")]
    public void CommandFinderShould_PutInGroup_CommandsTagged_Items_WithCorrectTagNames(int expected, string groupName)
    {
        var commands = GetCommands();
        var reader = new Mock<ISettingsReaderConfiguration>();
        var items = CommandFinder.ConvertToGroupedItems(new TestGroupBuilder(), commands, reader.Object, string.Empty).ToList();
        var actual = items.Where(x => x.Name == groupName).SelectMany(x => x.Items).Count(x => x.Tags!.Contains(groupName.ToLower()));

        Assert.NotEmpty(items);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("One", "one", 3)]
    [InlineData("One", "two", 1)]
    [InlineData("Two", "one", 1)]
    [InlineData("Two", "two", 2)]
    [InlineData("Three", "three", 1)]
    [InlineData("Four", "three", 1)]
    [InlineData("Four", "one", 3)]
    public void CommandFinderShould_PutInGroup_CommandsTag_Equals_Expected(string groupName, string tagName, int expected)
    {
        var commands = GetCommands();
        var reader = new Mock<ISettingsReaderConfiguration>();
        var items = CommandFinder.ConvertToGroupedItems(new TestGroupBuilder(), commands, reader.Object, string.Empty).ToList();
        var actual = items.Where(x => x.Name == groupName).SelectMany(x => x.Items).Count(x => x.Tags!.Contains(tagName));

        Assert.NotEmpty(items);
        Assert.Equal(expected, actual);
    }










    private IEnumerable<ICommandexCommand> GetCommands()
        => new List<ICommandexCommand>
        {
            new FakeCommandexCommand4(),
            new FakeCommandexCommand1(),
            new FakeCommandexCommand5(),
            new FakeCommandexCommand6(),
            new FakeCommandexCommand2(),
            new FakeCommandexCommand3(),
        };
}