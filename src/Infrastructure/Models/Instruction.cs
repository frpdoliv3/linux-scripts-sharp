using System.Diagnostics;

namespace Infrastructure.Models;

public class Instruction(string fileName)
{
    public string FileName { get; } = fileName;
    private readonly List<Argument> _mutableArgumentList = new();
    public IReadOnlyCollection<Argument> ArgumentList => _mutableArgumentList;
    public bool RedirectStandardInput { get; init; }
    public bool RedirectStandardOutput { get; init; }= true;
    public bool RedirectStandardError { get; init; }= true;
    public bool RequiresElevation { get; init; } = false;
    public Instruction? ChildInstruction { get; set; }

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
