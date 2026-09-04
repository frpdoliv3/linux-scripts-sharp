using Microsoft.Extensions.DependencyInjection;

namespace UI;

public class Navigator(IServiceScopeFactory serviceScopeFactory) : INavigator
{
    private record struct ScreenState(IScreen Screen, AsyncServiceScope ScreenScope);

    private readonly Stack<ScreenState> _screenStack = new();

    public IScreen? CurrentScreen
    {
        get
        {
            try
            {
                var screenState = _screenStack.Peek();
                return screenState.Screen;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }

    public void Push(ScreenType screenType)
    {
        var screenScope = serviceScopeFactory.CreateAsyncScope();
        var screen = screenScope.ServiceProvider.GetRequiredService(screenType.Value);
        _screenStack.Push(new ScreenState((IScreen) screen, screenScope));
    }

    public async Task PushReplacementAsync(ScreenType screenType)
    {
        await PopAsync();
        Push(screenType);
    }
    
    public async Task PopAsync()
    {
        var topOfStack = _screenStack.Pop();
        await topOfStack.ScreenScope.DisposeAsync();
    }

    public async Task ClearStack()
    {
        foreach (var screenEntry in _screenStack)
        {
            await screenEntry.ScreenScope.DisposeAsync();
        }
        _screenStack.Clear();
    }

    public async ValueTask DisposeAsync()
    {
        await ClearStack();
    }
}
