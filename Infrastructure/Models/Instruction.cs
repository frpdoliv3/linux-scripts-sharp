using System.Diagnostics;

namespace Infrastructure.FileSystem.Models;

public class Instruction
{
    public string FileName { get; }
    private readonly List<Argument> _mutableArgumentList = new();
    public IReadOnlyCollection<Argument> ArgumentList => _mutableArgumentList;
    public bool RedirectStandardInput { get; init; } = false;
    public bool RedirectStandardOutput { get; init; }= true;
    public bool RedirectStandardError { get; init; }= true;
    public Instruction? ChildInstruction { get; set; }

    public Instruction(string fileName)
    {
        FileName = fileName;
    }
    
    public Instruction AddArgument(Argument argument)
    {
        _mutableArgumentList.Add(argument);
        return this;
    }

    public Instruction AddArgumentRange(IEnumerable<Argument> arguments)
    {
        foreach (var argument in arguments)
        {
            AddArgument(argument);
        }

        return this;
    }

    public Instruction WrapInSudo(IEnumerable<Argument>? sudoArguments = null)
    {
        var sudoInstruction = new Instruction("sudo")
        {
            RedirectStandardInput = RedirectStandardInput,
            RedirectStandardOutput = RedirectStandardOutput,
            RedirectStandardError = RedirectStandardError,
            ChildInstruction = this
        };
        
        sudoInstruction.AddArgumentRange(sudoArguments ?? []);
        
        return sudoInstruction;
    }
    
    public ProcessStartInfo CreateProcessStartInfo()
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = FileName,
            RedirectStandardInput = RedirectStandardInput,
            RedirectStandardOutput = RedirectStandardOutput,
            RedirectStandardError = RedirectStandardError
        };

        var curInstruction = this;
        while (curInstruction != null)
        {
            if (curInstruction != this)
            {
                processStartInfo.ArgumentList.Add(curInstruction.FileName);
            }

            var flattenedArguments = curInstruction.ArgumentList
                .SelectMany(argument => argument.ToProcessArguments());
            foreach (var argument in flattenedArguments)
            {
                processStartInfo.ArgumentList.Add(argument);
            }
            
            curInstruction = curInstruction.ChildInstruction;
        }

        return processStartInfo;
    }
}
