using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Infrastructure.OSOperations;

public class LinuxOSOperationProvider(ILogger<LinuxOSOperationProvider> logger) : OSOperationProvider(logger)
{
    public override IAsyncEnumerable<string> ReadFileByLineAsync(
        string path, 
        CancellationToken cancellationToken = default
    ) => File.ReadLinesAsync(path, cancellationToken);
    
    protected override async Task<ProcessResult?> SpawnProcessAsync(
        ProcessStartInfo processStartInfo,
        CancellationToken cancellationToken = default
    ) {
        var process = Process.Start(processStartInfo);
        if (process == null)
        {
            return null;
        }

        await process.WaitForExitAsync(cancellationToken);

        return new ProcessResult(
            process.ExitCode,
            process.StandardOutput,
            process.StandardError
        );
    }
}
