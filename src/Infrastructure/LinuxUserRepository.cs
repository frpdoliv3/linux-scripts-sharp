using Infrastructure.Models;
using Infrastructure.OSOperations;

namespace Infrastructure;

public enum CreateUserResult
{
    UserCreated,
    UserAlreadyExists,
    UserCreationFailed
}

public class LinuxUserRepository(IOSOperationProvider osOperationProvider, LinuxPaths linuxPaths)
{
    public async Task<IEnumerable<string>> GetUsers(CancellationToken cancellationToken = default) => 
        await osOperationProvider.ReadFileByLineAsync(linuxPaths.Passwd, cancellationToken)
            .Select(userRow => userRow.Split(":")[0])
            .ToListAsync(cancellationToken);
    
    public async Task<CreateUserResult> CreateSystemUser(
        string username,
        string homeBaseDir = "/var/lib",
        CancellationToken cancellationToken = default
    ) {
        var normalizedHomeBaseDir = homeBaseDir switch
        {
            [.. var prefix, '/'] => prefix,
            _ => homeBaseDir
        };
        
        var createUserInstruction = new Instruction("useradd")
            {
                RequiresElevation = true
            } 
            .AddArgument(Argument.CreateLongArgument("base-dir", normalizedHomeBaseDir))
            .AddArgument(Argument.CreateLongArgument("create-home"))
            .AddArgument(Argument.CreateLongArgument("system"))
            .AddArgument(Argument.CreateLongArgument("shell", "/sbin/nologin"))
            .AddArgument(Argument.CreatePositionalArgument(username));
        
        var instructionResult = await osOperationProvider.RunInstructionAsync(createUserInstruction, cancellationToken);
        return instructionResult switch
        {
            InstructionSucceeded => CreateUserResult.UserCreated,
            InstructionExitedWithError { ExitCode: 9 } => CreateUserResult.UserAlreadyExists,
            InstructionFailedToStart or InstructionExitedWithError => CreateUserResult.UserCreationFailed,
            _ => throw new NotImplementedException()
        };
    }
}
