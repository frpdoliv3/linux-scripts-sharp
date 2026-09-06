using Spectre.Console;

namespace UI.Screens;

public class SetupTorrentScreen : IScreen
{
    public Task ShowAsync()
    {
        var userName = AnsiConsole.Ask<string>("Choose a username for the service owner");
        return Task.CompletedTask;
    }
}
