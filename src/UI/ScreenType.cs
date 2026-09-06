namespace UI;

public readonly record struct ScreenType
{
    public Type Value { get; }

    private ScreenType(Type value)
    {
        Value = value;
    }

    public static ScreenType Of<TScreen>()
        where TScreen : IScreen
        => new(typeof(TScreen));
}
