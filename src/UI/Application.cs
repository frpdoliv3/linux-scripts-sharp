namespace UI;

public class Application(INavigator navigator)
{
    public async Task RunAsync(ScreenType initScreenType)
    {
        navigator.Push(initScreenType);
        while (navigator.CurrentScreen != null)
        {
            await navigator.CurrentScreen.ShowAsync();
        }
    }
}
