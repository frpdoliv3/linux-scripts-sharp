using System.Text;
using AwesomeAssertions;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using Microsoft.Extensions.Logging.Abstractions;

namespace Infrastructure.IntegrationTests;

public class LinuxUserRepositoryTests(ArchLinuxTestingImageFixture imageFixture)
    : IClassFixture<ArchLinuxTestingImageFixture>, IAsyncLifetime
{
    private IContainer _container;

    public async ValueTask InitializeAsync()
    {
        var container = new ContainerBuilder(imageFixture.ArchLinuxTestingImage)
            .WithCleanUp(true)
            .Build();
        await container.StartAsync();
        
        _container = container;
    }

    [Fact]
    public async Task MustContainBaseUsers()
    {
        var linuxUserRepository = new LinuxUserRepository(new ContainerOSOperationProvider(NullLogger<ContainerOSOperationProvider>.Instance, _container), new LinuxPaths());

        var usersEnumerable = await linuxUserRepository
            .GetUsers(TestContext.Current.CancellationToken);
        var usersList = usersEnumerable.ToList();
        
        usersList.Should().Contain("root");
        usersList.Should().Contain(ArchLinuxTestingImageFixture.TestUser);
    }
    
    public async ValueTask DisposeAsync()
        => await _container.DisposeAsync();
}
