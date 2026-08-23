using Infrastructure.Models;

namespace Infrastructure.OSOperations;

public interface IOSOperationProvider
{
    IAsyncEnumerable<string> ReadFileByLineAsync(string path, CancellationToken cancellationToken = default);
    Task<InstructionResult> RunInstructionAsync(Instruction instruction, CancellationToken cancellationToken = default);
}