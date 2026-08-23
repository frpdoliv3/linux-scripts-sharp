using Infrastructure.Models;
using Infrastructure.OSOperations;

namespace Infrastructure;

public enum CreateUserResult
{
    UserCreated,
    UserAlreadyExists,
    UserCreationFailed
}

public class LinuxUserRepository(OSOperationProvider osOperationProvider, LinuxPaths linuxPaths)
{
    public async Task<IEnumerable<string>> GetUsers(CancellationToken cancellationToken = default) => 
        await osOperationProvider.ReadFileByLineAsync(linuxPaths.Passwd, cancellationToken)
            .Select(userRow => userRow.Split(":")[0])
            .ToListAsync(cancellationToken);

    public async Task<CreateUserResult> CreateUser(string username, CancellationToken cancellationToken = default)
    {
        var createUserInstruction = new Instruction("useradd")
            {
                RequiresElevation = true
            }
            .AddArgument(Argument.CreateLongArgument("base-dir", "/var/lib"))
            .AddArgument(Argument.CreateLongArgument("create-home"))
            .AddArgument(Argument.CreateLongArgument("system"))
            .AddArgument(Argument.CreateLongArgument("shell", "/sbin/nologin"))
            .AddArgument(Argument.CreatePositionalArgument("qbittorrent"));
        
        var instructionResult = await osOperationProvider.RunInstructionAsync(createUserInstruction);
        return instructionResult switch
        {
            InstructionSucceeded => CreateUserResult.UserCreated,
            InstructionFailedToStart or InstructionExitedWithError => CreateUserResult.UserCreationFailed,
            _ => throw new NotImplementedException()
        };
    }
}
