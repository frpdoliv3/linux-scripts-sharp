using System.Diagnostics;
using Infrastructure.Models;
using Microsoft.Extensions.Logging;

namespace Infrastructure.OSOperations;

public abstract record InstructionResult;

public sealed record InstructionSucceeded(
    string StandardOutput,
    string StandardError
) : InstructionResult;

public sealed record InstructionFailedToStart(
    string Message
) : InstructionResult;

public sealed record InstructionExitedWithError(
    int ExitCode,
    string StandardOutput,
    string StandardError
) : InstructionResult;

public abstract class OSOperationProvider(ILogger<OSOperationProvider> logger)
{
    public async Task<InstructionResult> RunInstructionAsync(Instruction instruction)
    {
        if (instruction.RequiresElevation)
        {
            instruction = instruction.WrapInSudo();
        }
        
        var processStartInfo = instruction.CreateProcessStartInfo();
        
        var processResult = await SpawnProcessAsync(processStartInfo);
        if (processResult == null)
        {
            logger.LogError("Could not create child process for instruction {Instruction}",  instruction);
            return new InstructionFailedToStart(
                $"Could not create child process for instruction: {instruction.FileName}");
        }

        var standardOutputString = await processResult.StandardOutput.ReadToEndAsync();
        var standardErrorString = await processResult.StandardError.ReadToEndAsync();
        
        if (processResult.ExitCode != 0)
        {
            if (!string.IsNullOrEmpty(standardErrorString))
            {
                logger.LogError("Child process exited with error code {ExitCode} and message {StandardErrorMessage}", processResult.ExitCode, standardErrorString);    
            }
            else
            {
                logger.LogError("Child process exited with error code {ExitCode} and message {StandardErrorMessage}", processResult.ExitCode, standardErrorString);
            }

            return new InstructionExitedWithError(
                processResult.ExitCode,
                standardOutputString,
                standardErrorString
            );
        }
        
        logger.LogInformation("Child process exited with standard output: {StandardOutputMessage}", standardOutputString);
        return new InstructionSucceeded(
            standardOutputString,
            standardErrorString
        );
    }
    
    public abstract IAsyncEnumerable<string> ReadFileByLineAsync(
        string path,
        CancellationToken cancellationToken = default
    );
    
    protected abstract Task<ProcessResult?> SpawnProcessAsync(
        ProcessStartInfo processStartInfo,
        CancellationToken cancellationToken = default
    );

    protected Instruction ElevateInstruction(Instruction instruction)
        => instruction.WrapInSudo();
    
    protected record ProcessResult(
        int ExitCode,
        StreamReader StandardOutput,
        StreamReader StandardError
    );
}