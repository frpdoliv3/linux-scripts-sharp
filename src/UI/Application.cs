namespace UI;

public class Application
{
    public async Task RunAsync(IScreen entryPoint)
    {
        var curScreen = entryPoint;
        while (curScreen != null)
        {
            curScreen = await entryPoint.ShowAsync();
        }
    }
}
