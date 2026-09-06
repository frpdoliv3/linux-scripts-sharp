namespace UI;

public interface INavigator : IAsyncDisposable
{
    IScreen? CurrentScreen { get; }
    void Push(ScreenType screenType);
    Task PushReplacementAsync(ScreenType screenType);
    Task PopAsync();
    Task ClearStack();
}
