using System.IO;

public static class ImportData
{
    public static void FillDataBase(string pathToCsvFile, GameRepository gameRepository, PlatformRepository platformRepository, ConnectionRepository connectionRepository)
    {
        StreamReader reader = new StreamReader(pathToCsvFile);
        string line = "";
        while (true)
        {
            line = reader.ReadLine();
            if (line == null)
            {
                break;
            }
            string[] lineParts = line.Split(",");
            ParseLine(lineParts, gameRepository, platformRepository, connectionRepository);
        }
        reader.Close();

    }

    private static void ParseLine(string[] lineParts, GameRepository gameRepository, PlatformRepository platformRepository, ConnectionRepository connectionRepository)
    {
        Game game = new Game();
        Platform platform = new Platform();

        bool isYear = int.TryParse(lineParts[3], out game.year);
        bool isGlobalSales = double.TryParse(lineParts[10], out game.globalSales);
        if (isYear && isGlobalSales)
        {
            game.name = lineParts[1];
            game.year = int.Parse(lineParts[3]);
            game.globalSales = double.Parse(lineParts[10]);
            platform.name = lineParts[2];
        }
        else if (lineParts[3] == "N/A")
        {
            game.name = lineParts[1];
            game.year = 0;
            game.globalSales = double.Parse(lineParts[10]);
            platform.name = lineParts[2];

        }
        AddDataToBD(game, platform, gameRepository, platformRepository, connectionRepository);


    }

    private static void AddDataToBD(Game game, Platform platform, GameRepository gameRepository, PlatformRepository platformRepository, ConnectionRepository connectionRepository)
    {

        Connection gameAndPlatformConnection = new Connection();

        if (gameRepository.GameExists(game.name) == 0)
        {
            gameAndPlatformConnection.gameId = gameRepository.Insert(game);

        }
        else
        {
            gameAndPlatformConnection.gameId = gameRepository.GameExists(game.name);
        }
        if (platformRepository.PlatformExists(platform.name) == 0)
        {
            gameAndPlatformConnection.platformId = platformRepository.Insert(platform);

        }
        else
        {
            gameAndPlatformConnection.platformId = platformRepository.PlatformExists(platform.name);

        }
        connectionRepository.Insert(gameAndPlatformConnection);


    }
}
