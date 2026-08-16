using Infrastructure.FileSystem;

var repo = new LinuxUserRepository(new LinuxPaths());

var runScript = false;
Console.Write("Are you sure you want to run this script (y/n (default)): ");
var userInput = Console.ReadLine();

if (userInput.ToUpper() == "Y")
{
    runScript = true;
}

if (runScript)
{
    await repo.CreateUser("qbittorrent");
}
