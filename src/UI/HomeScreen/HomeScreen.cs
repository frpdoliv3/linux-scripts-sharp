using Spectre.Console;

namespace UI.Screens;

public class HomeScreen(INavigator navigator) : IScreen
{
    private enum HomeOption
    {
        SetupTorrent,
        ConfigureSecureBoot,
        ConfigureTpm2
    } 
    
    public async Task ShowAsync()
    {
        var title = new FigletText("Linux Setup")
            .Color(Color.Cyan1);

        var welcome = new Panel(title)
            .Border(BoxBorder.Rounded)
            .BorderColor(Color.Cyan1)
            .Padding(1, 1);

        AnsiConsole.Write(Align.Center(welcome));

        var option = AnsiConsole.Prompt(
            new SelectionPrompt<HomeOption>()
                .Title("[bold]Choose an action:[/]")
                .PageSize(10)
                .HighlightStyle(new Style(
                    foreground: Color.Cyan1,
                    decoration: Decoration.Bold))
                .UseConverter(option => option switch
                {
                    HomeOption.SetupTorrent => "Setup qBittorrent+Gluetun"
                })
                .AddChoices(
                    HomeOption.SetupTorrent));


        switch (option)
        {
            case HomeOption.SetupTorrent:
                navigator.Push(ScreenType.Of<SetupTorrentScreen>());
                break;
            default:
                await navigator.ClearStack();
                break;
        }
    }
}
