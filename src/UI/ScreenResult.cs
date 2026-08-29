namespace UI;

public abstract record ScreenResult
{
    public sealed record Navigate<TScreen> : ScreenResult
        where TScreen : IScreen;  
    
    public sealed record Exit : ScreenResult;
}

public class ScreenResolver
{
    public IScreen? ResolveScreen(ScreenResult? result)
        => result switch
        {
            ScreenResult.Navigate => new(typeof(T));
            ScreenResult.Exit or _ => null
        }
}