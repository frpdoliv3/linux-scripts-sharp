using System.Diagnostics;
using System.Runtime.CompilerServices;
using DotNet.Testcontainers.Containers;
using Infrastructure.OSOperations;
using Microsoft.Extensions.Logging;

namespace Infrastructure.IntegrationTests;

public class ContainerOSOperationProvider(ILogger<OSOperationProvider> logger, IContainer container) : OSOperationProvider(logger)
{
    public override async IAsyncEnumerable<string> ReadFileByLineAsync(string path,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var fileBytes = await container.ReadFileAsync(path, cancellationToken);
        
        using var stream = new MemoryStream(fileBytes);
        using var reader = new StreamReader(stream);

        while (await reader.ReadLineAsync(cancellationToken) is { } line)
        {
            yield return line;
        }
        
    }
    
    protected override async Task<ProcessResult?> SpawnProcessAsync(ProcessStartInfo processStartInfo, CancellationToken cancellationToken = default)
    {
        List<string> fullCommand =
        [
            processStartInfo.FileName,
            .. processStartInfo.ArgumentList
        ];

        var commandResult = await container.ExecAsync(fullCommand, cancellationToken);
        if (commandResult.ExitCode == null)
        {
            return null;
        }
        
        return new ProcessResult(
            (int) commandResult.ExitCode ,
            new StringReader(commandResult.Stdout),
            new StringReader(commandResult.Stderr)
        );
    }
}