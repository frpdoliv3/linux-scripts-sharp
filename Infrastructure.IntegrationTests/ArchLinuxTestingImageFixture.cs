using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Images;

namespace Infrastructure.IntegrationTests;

public class ArchLinuxTestingImageFixture : IAsyncLifetime
{
    public const string TestUser = "archlinux-testing";
    private const string TestPassword = "archlinux-testing";
    
    public IFutureDockerImage ArchLinuxTestingImage { get; private set; }
    
    public async ValueTask InitializeAsync()
    {
        var futureArchImage = new ImageFromDockerfileBuilder()
            .WithName("archlinux-testing")
            .WithDockerfile("Resources/Dockerfile")
            .WithBuildArgument("TEST_USER", TestUser)
            .WithBuildArgument("TEST_PASSWORD", TestPassword)
            .WithDeleteIfExists(true)
            .WithCleanUp(true)
            .Build();

        await futureArchImage.CreateAsync()
            .ConfigureAwait(false);
        
        ArchLinuxTestingImage = futureArchImage;
    }

    public async ValueTask DisposeAsync()
        => await ArchLinuxTestingImage.DeleteAsync();
}