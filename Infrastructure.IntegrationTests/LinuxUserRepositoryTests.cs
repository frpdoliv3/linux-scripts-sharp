using System.Text;
using AwesomeAssertions;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
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
        var linuxUserRepository = new LinuxUserRepository(
            new ContainerOSOperationProvider(NullLogger<ContainerOSOperationProvider>.Instance, _container), 
            new LinuxPaths()
        );

        var usersEnumerable = await linuxUserRepository
            .GetUsers(TestContext.Current.CancellationToken);
        var usersList = usersEnumerable.ToList();
        
        usersList.Should().Contain("root");
        usersList.Should().Contain(ArchLinuxTestingImageFixture.TestUser);
    }

    [Fact]
    public async Task MustCreateNewUser()
    {
        var testUserName = "gervasio";
        var linuxPaths = new LinuxPaths();
        
        var linuxUserRepository = new LinuxUserRepository(
            new ContainerOSOperationProvider(NullLogger<ContainerOSOperationProvider>.Instance, _container), 
            linuxPaths
        );

        var userCreateResult = await linuxUserRepository
            .CreateUser(testUserName, TestContext.Current.CancellationToken);
        
        userCreateResult.Should().Be(CreateUserResult.UserCreated);
        var fileByteContent = await _container
            .ReadFileAsync(linuxPaths.Passwd, TestContext.Current.CancellationToken);
        
        using var memoryStream = new MemoryStream(fileByteContent);
        using var streamReader = new StreamReader(memoryStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);

        var passwdContentString = await streamReader.ReadToEndAsync(TestContext.Current.CancellationToken);
        passwdContentString.Should().Contain(testUserName);
    }
    
    public async ValueTask DisposeAsync()
        => await _container.DisposeAsync();
}
