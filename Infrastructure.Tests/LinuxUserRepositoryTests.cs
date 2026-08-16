using AwesomeAssertions;
using Infrastructure.FileSystem;

namespace Infrastructure.Tests;

public class LinuxUserRepositoryTests
{
    [Fact]
    public async Task MustListOSUsersInOrder()
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
        
        var userRepo = new FileSystem.LinuxUserRepository(new LinuxPaths(
            Passwd: "Resources/edge-case-passwd"
        ));

        var users = await userRepo.GetUsers();

        users.Should().Equal(userNames);
    }
}
