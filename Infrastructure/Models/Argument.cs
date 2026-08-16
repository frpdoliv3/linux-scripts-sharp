namespace Infrastructure.FileSystem.Models;

public enum ArgumentType
{
    Positional,
    ShortOption,
    LongOption,
}

public class Argument
{
    private string Name { get; }
    private string? Value { get; }
    private ArgumentType Type { get; }

    private Argument(string name, ArgumentType type, string? value = null)
    {
        Name = name;
        Value = value;
        Type = type;
    }

    public static Argument CreateShortArgument(string name, string? value = null)
    {
        return new Argument(
            name: name,
            value: value,
            type: ArgumentType.ShortOption
        );
    }
    
    public static Argument CreateLongArgument(string name, string? value = null)
    {
        return new Argument(
            name: name,
            value: value,
            type: ArgumentType.LongOption
        );
    }
    
    public static Argument CreatePositionalArgument(string name)
    {
        return new Argument(
            name: name,
            type: ArgumentType.Positional
        );
    }
    
    public IEnumerable<string> ToProcessArguments() => Type switch
    {
        ArgumentType.Positional => [Name],
        ArgumentType.ShortOption => Value is null ? [$"-{Name}"] : [$"-{Name}", Value],
        ArgumentType.LongOption => Value is null ? [$"--{Name}"] : [$"--{Name}", Value],
        _ => throw new ArgumentOutOfRangeException()
    };
}
