using Microsoft.Data.Sqlite;
using Terminal.Gui;

class ProgramF
{
    static void Main(string[] args)
    {
        string databaseFileName = "./dataBase";
        SqliteConnection connection = new SqliteConnection($"Data Source={databaseFileName}");
        GameRepository gameRepository = new GameRepository(connection);
        PlatformRepository platformRepository = new PlatformRepository(connection);
        ConnectionRepository connectionRepository = new ConnectionRepository(connection);

        Application.Init();
        Toplevel top = Application.Top;
        MainWindow window = new MainWindow();
        window.SetData(gameRepository, platformRepository, connectionRepository);
        top.Add(window);

        Application.Run();
    }


}

