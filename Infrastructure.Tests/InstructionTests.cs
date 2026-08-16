using System.Diagnostics;
using AwesomeAssertions;
using Infrastructure.FileSystem.Models;

namespace Infrastructure.Tests;

public class InstructionTests
{
    [Fact]
    public void MustCreateSingleInstruction()
    {
        var instruction = new Instruction("echo");

        var processInfo = instruction.CreateProcessStartInfo();
        
        processInfo.FileName.Should().Be("echo");
    }

    [Fact]
    public void MustWrapInSudo()
    {
        var instruction = new Instruction("echo");

        instruction = instruction.WrapInSudo();
        
        var processInfo = instruction.CreateProcessStartInfo();
        
        processInfo.FileName.Should().Be("sudo");
        processInfo.ArgumentList.Should().Equal(["echo"]);
    }

    [Fact]
    public void MustHandleMultipleArguments()
    {
        IEnumerable<string> expectedArgumentList =
        [
            "--base-dir", "/var/lib/",
            "--create-home",
            "-s", "/sbin/nologin",
            "test-user"
        ];
        
        var instruction = new Instruction("useradd")
            .AddArgument(Argument.CreateLongArgument("base-dir", "/var/lib/"))
            .AddArgument(Argument.CreateLongArgument("create-home"))
            .AddArgument(Argument.CreateShortArgument("s", "/sbin/nologin"))
            .AddArgument(Argument.CreatePositionalArgument("test-user"));
        
        var processInfo = instruction.CreateProcessStartInfo();
        
        processInfo.FileName.Should().Be("useradd");
        processInfo.ArgumentList.Should().Equal(expectedArgumentList);
    }
    
    [Fact]
    public void MustHandleMultipleArgumentsWrappedInSudo()
    {
        IEnumerable<string> expectedArgumentList =
        [
            "useradd",
            "--base-dir", "/var/lib/",
            "--create-home",
            "-s", "/sbin/nologin",
            "test-user"
        ];
        
        var instruction = new Instruction("useradd")
            .AddArgument(Argument.CreateLongArgument("base-dir", "/var/lib/"))
            .AddArgument(Argument.CreateLongArgument("create-home"))
            .AddArgument(Argument.CreateShortArgument("s", "/sbin/nologin"))
            .AddArgument(Argument.CreatePositionalArgument("test-user"));
        
        instruction = instruction.WrapInSudo();
        
        var processInfo = instruction.CreateProcessStartInfo();
        
        processInfo.FileName.Should().Be("sudo");
        processInfo.ArgumentList.Should().Equal(expectedArgumentList);
    }
    
    [Fact]
    public void MustHandleMultipleArgumentsWrappedInSudoWithArguments()
    {
        IEnumerable<string> expectedArgumentList =
        [
            "-u", "test-user1",
            "useradd",
            "--base-dir", "/var/lib/",
            "--create-home",
            "-s", "/sbin/nologin",
            "test-user2"
        ];
        
        var instruction = new Instruction("useradd")
            .AddArgument(Argument.CreateLongArgument("base-dir", "/var/lib/"))
            .AddArgument(Argument.CreateLongArgument("create-home"))
            .AddArgument(Argument.CreateShortArgument("s", "/sbin/nologin"))
            .AddArgument(Argument.CreatePositionalArgument("test-user2"));
        
        instruction = instruction.WrapInSudo([Argument.CreateShortArgument("u", "test-user1")]);
        
        var processInfo = instruction.CreateProcessStartInfo();
        
        processInfo.FileName.Should().Be("sudo");
        processInfo.ArgumentList.Should().Equal(expectedArgumentList);
    }
}