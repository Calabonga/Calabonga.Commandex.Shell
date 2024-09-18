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

    [Fact]
    public void CommandFinderShould_PutInGroupOne_CommandsTagged_One_Equals_3()
    {
        const int expected = 3;

        var commands = GetCommands();
        var reader = new Mock<ISettingsReaderConfiguration>();
        var items = CommandFinder.ConvertToGroupedItems(new TestGroupBuilder(), commands, reader.Object, string.Empty).ToList();
        var actual = items.Where(x => x.Name == "One").SelectMany(x => x.Items).Count(x => x.Tags!.Contains("one"));

        Assert.NotEmpty(items);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CommandFinderShould_PutInGroupOne_CommandsTagged_Two_Equals_2()
    {
        const int expected = 2;

        var commands = GetCommands();
        var reader = new Mock<ISettingsReaderConfiguration>();
        var items = CommandFinder.ConvertToGroupedItems(new TestGroupBuilder(), commands, reader.Object, string.Empty).ToList();
        var actual = items.Where(x => x.Name == "Two").SelectMany(x => x.Items).Count(x => x.Tags!.Contains("two"));

        Assert.NotEmpty(items);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CommandFinderShould_PutInGroupOne_CommandsTagged_Three_Equals_1()
    {
        const int expected = 1;

        var commands = GetCommands();
        var reader = new Mock<ISettingsReaderConfiguration>();
        var items = CommandFinder.ConvertToGroupedItems(new TestGroupBuilder(), commands, reader.Object, string.Empty).ToList();
        var actual = items.Where(x => x.Name == "Three").SelectMany(x => x.Items).Count(x => x.Tags!.Contains("three"));

        Assert.NotEmpty(items);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CommandFinderShould_PutInGroupFour_CommandsTagged_Three_Equals_1()
    {
        const int expected = 1;

        var commands = GetCommands();
        var reader = new Mock<ISettingsReaderConfiguration>();
        var items = CommandFinder.ConvertToGroupedItems(new TestGroupBuilder(), commands, reader.Object, string.Empty).ToList();
        var actual = items.Where(x => x.Name == "Four").SelectMany(x => x.Items).Count(x => x.Tags!.Contains("three"));

        Assert.NotEmpty(items);
        Assert.Equal(expected, actual);
    }
    [Fact]
    public void CommandFinderShould_PutInGroupFour_CommandsTagged_One_Equals_1()
    {
        const int expected = 3;

        var commands = GetCommands();
        var reader = new Mock<ISettingsReaderConfiguration>();
        var items = CommandFinder.ConvertToGroupedItems(new TestGroupBuilder(), commands, reader.Object, string.Empty).ToList();
        var actual = items.Where(x => x.Name == "Four").SelectMany(x => x.Items).Count(x => x.Tags!.Contains("one"));

        Assert.NotEmpty(items);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CommandFinderShould_PutInGroupOne_CommandsTagged_Three_Equals()
    {
        const int expected = 1;

        var commands = GetCommands();
        var reader = new Mock<ISettingsReaderConfiguration>();
        var items = CommandFinder.ConvertToGroupedItems(new TestGroupBuilder(), commands, reader.Object, string.Empty).ToList();
        var actual = items.Where(x => x.Name == "Three").SelectMany(x => x.Items).Count(x => x.Tags!.Contains("three"));

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