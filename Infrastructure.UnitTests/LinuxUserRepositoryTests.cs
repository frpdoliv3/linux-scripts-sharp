using System.Diagnostics;
using AwesomeAssertions;
using Infrastructure.OSOperations;
using Microsoft.Extensions.Logging.Abstractions;

namespace Infrastructure.UnitTests;

public class LinuxUserRepositoryTests
{
    private class ReadonlyOSOperationProvider() : OSOperationProvider(NullLogger<OSOperationProvider>.Instance)
    {
        public override IAsyncEnumerable<string> ReadFileByLineAsync(
            string path,
            CancellationToken cancellationToken = default
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
}
