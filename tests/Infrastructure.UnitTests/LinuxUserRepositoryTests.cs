using System.Diagnostics;
using AwesomeAssertions;
using Infrastructure.Models;
using Infrastructure.OSOperations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace Infrastructure.UnitTests;

public class LinuxUserRepositoryTests
{
    private class ReadonlyOSOperationProvider() : OSOperationProvider(NullLogger<OSOperationProvider>.Instance)
    {
        public override IAsyncEnumerable<string> ReadFileByLineAsync(
            string path,
            CancellationToken cancellationToken
        ) => File.ReadLinesAsync(path, cancellationToken);

        protected override Task<ProcessResult?> SpawnProcessAsync(ProcessStartInfo processStartInfo, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
    
    [Fact]
    public async Task MustListOsUsersInOrder()
    {
        var userNames = new List<string>
        {
            "root",
            "shadowed",
            "passwordless",
            "locked",
            "asterisk",
            "nisplus",
            "encrypted",
            "empty_gecos",
            "empty_shell",
            "nonexistent_shell",
            "UpperCase"
        };

        var linuxPaths = new LinuxPaths(Passwd: "Resources/edge-case-passwd");
        var userRepo = new LinuxUserRepository(new ReadonlyOSOperationProvider(), linuxPaths);

        var users = await userRepo.GetUsers(TestContext.Current.CancellationToken);

        users.Should().Equal(userNames);
    }
    
    [Fact]
    public async Task MustUseDefaultHome()
    {
        Instruction? capturedInstruction = null;
        var testUserName = "test-user";  
        
        var osOperationProvider = Substitute.For<IOSOperationProvider>();
        osOperationProvider.RunInstructionAsync(
            Arg.Do<Instruction>(inst => capturedInstruction = inst), 
            Arg.Any<CancellationToken>()
        ).Returns(new InstructionSucceeded("", ""));

        var userRepo = new LinuxUserRepository(osOperationProvider, new LinuxPaths());
        await userRepo.CreateSystemUser(testUserName, cancellationToken: TestContext.Current.CancellationToken);
        
        capturedInstruction.Should().NotBeNull();

        var createdUserBaseDir = capturedInstruction.ArgumentList.First(args => args is
            { Name: "base-dir", Type: ArgumentType.LongOption } or
            { Name: "b", Type: ArgumentType.ShortOption }
        ).Value;

        createdUserBaseDir.Should().NotBeNull();
        createdUserBaseDir.Should().Be("/var/lib");
    }
    
    [Fact]
    public async Task MustUseProvidedHome()
    {
        Instruction? capturedInstruction = null;
        var testUserName = "test-user";
        var testBaseHomeDir = "/home";
        
        var osOperationProvider = Substitute.For<IOSOperationProvider>();
        osOperationProvider.RunInstructionAsync(
            Arg.Do<Instruction>(inst => capturedInstruction = inst), 
            Arg.Any<CancellationToken>()
        ).Returns(new InstructionSucceeded("", ""));

        var userRepo = new LinuxUserRepository(osOperationProvider, new LinuxPaths());
        await userRepo.CreateSystemUser(testUserName, testBaseHomeDir, cancellationToken: TestContext.Current.CancellationToken);
        
        capturedInstruction.Should().NotBeNull();

        var createdUserBaseDir = capturedInstruction.ArgumentList.First(args => args is
            { Name: "base-dir", Type: ArgumentType.LongOption } or
            { Name: "b", Type: ArgumentType.ShortOption }
        ).Value;

        createdUserBaseDir.Should().NotBeNull();
        createdUserBaseDir.Should().Be(testBaseHomeDir);
    }
    
    [Fact]
    public async Task MustUseProvidedHomeAndTrimTrailingSlash()
    {
        Instruction? capturedInstruction = null;
        var testUserName = "test-user";
        var testBaseHomeDir = "/home/";
        
        var osOperationProvider = Substitute.For<IOSOperationProvider>();
        osOperationProvider.RunInstructionAsync(
            Arg.Do<Instruction>(inst => capturedInstruction = inst), 
            Arg.Any<CancellationToken>()
        ).Returns(new InstructionSucceeded("", ""));

        var userRepo = new LinuxUserRepository(osOperationProvider, new LinuxPaths());
        await userRepo.CreateSystemUser(testUserName, testBaseHomeDir, cancellationToken: TestContext.Current.CancellationToken);
        
        capturedInstruction.Should().NotBeNull();

        var createdUserBaseDir = capturedInstruction.ArgumentList.First(args => args is
            { Name: "base-dir", Type: ArgumentType.LongOption } or
            { Name: "b", Type: ArgumentType.ShortOption }
        ).Value;

        createdUserBaseDir.Should().NotBeNull();
        createdUserBaseDir.Should().Be("/home");
    }
}
