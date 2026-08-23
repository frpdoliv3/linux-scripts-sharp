using Infrastructure.Models;

namespace Infrastructure.OSOperations;

public static class InstructionExtensions
{
    public static Instruction WrapInSudo(this Instruction instruction, IEnumerable<Argument>? sudoArguments = null)
    {
        var sudoInstruction = new Instruction("sudo")
        {
            RedirectStandardInput = instruction.RedirectStandardInput,
            RedirectStandardOutput = instruction.RedirectStandardOutput,
            RedirectStandardError = instruction.RedirectStandardError,
            ChildInstruction = instruction
        };
        
        sudoInstruction.AddArgumentRange(sudoArguments ?? []);
        
        return sudoInstruction;
    }
}