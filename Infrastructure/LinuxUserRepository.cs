using System.Diagnostics;

namespace Infrastructure.FileSystem;

public class LinuxUserRepository(LinuxPaths linuxPaths)
{
    public async Task<IEnumerable<string>> GetUsers(CancellationToken cancellationToken = default)
    {
        return await File.ReadLinesAsync(linuxPaths.Passwd, cancellationToken)
            .Select(userRow => userRow.Split(":")[0])
            .ToListAsync(cancellationToken);
    }

    public async Task CreateUser(string username, CancellationToken cancellationToken = default)
    {
        var startInfo = new ProcessStartInfo
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            FileName = "sudo",
            ArgumentList = { "useradd", "--base-dir", "/var/lib/", "--create-home", "--system", "--shell", "/sbin/nologin", "qbittorrent" }
        };
        
        var process = Process.Start(startInfo);
        await process.WaitForExitAsync(cancellationToken);

        var stderr = await process.StandardError.ReadToEndAsync(cancellationToken);
        var stdout = await process.StandardOutput.ReadToEndAsync(cancellationToken);

        Console.WriteLine("Standard Output: ");
        Console.WriteLine(stdout);
        
        Console.WriteLine("\nStandard Error: ");
        Console.WriteLine(stderr);
    }
}
