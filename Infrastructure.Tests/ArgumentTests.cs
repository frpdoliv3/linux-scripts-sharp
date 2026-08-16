using AwesomeAssertions;
using Infrastructure.FileSystem.Models;

namespace Infrastructure.Tests;

public class ArgumentTests
{
    [Fact]
    public void MustCreateShortArgument()
    {
        var argument = Argument.CreateShortArgument("la");
        var argumentEnumerable = argument.ToProcessArguments();

        argumentEnumerable.Should().Equal(["-la"]);
    }
    
    [Fact]
    public void MustCreateShortArgumentWithValue()
    {
        var argument = Argument.CreateShortArgument("m", "/home/test-user");
        var argumentEnumerable = argument.ToProcessArguments();

        argumentEnumerable.Should().Equal(["-m", "/home/test-user"]);
    }
    
    [Fact]
    public void MustCreateLongArgument()
    {
        var argument = Argument.CreateLongArgument("create-home");
        var argumentEnumerable = argument.ToProcessArguments();

        argumentEnumerable.Should().Equal(["--create-home"]);
    }
    
    [Fact]
    public void MustCreateLongArgumentWithValue()
    {
        var argument = Argument.CreateLongArgument("create-home", "/home/test-user");
        var argumentEnumerable = argument.ToProcessArguments();

        argumentEnumerable.Should().Equal(["--create-home", "/home/test-user"]);
    }
    
    [Fact]
    public void MustCreateLongArgumentWithValueDirectAssignment()
    {
        var argument = Argument.CreateLongArgument("create-home=/home/test-user");
        var argumentEnumerable = argument.ToProcessArguments();

        argumentEnumerable.Should().Equal(["--create-home=/home/test-user"]);
    }
    
    [Fact]
    public void MustCreatePositionalArgument()
    {
        var argument = Argument.CreatePositionalArgument("test-user");
        var argumentEnumerable = argument.ToProcessArguments();

        argumentEnumerable.Should().Equal(["test-user"]);
    }
}