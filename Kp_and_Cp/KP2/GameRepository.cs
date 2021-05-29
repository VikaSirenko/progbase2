using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
public class GameRepository
{
    private SqliteConnection connection;
    public GameRepository(SqliteConnection connection)
    {
        this.connection = connection;
    }

    public long GameExists(string name)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM games WHERE name = $name";
        command.Parameters.AddWithValue("$name", name);
        SqliteDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            Game game = new Game();
            game.id = long.Parse(reader.GetString(0));
            return game.id;
        }
        connection.Close();
        return 0;

    }

    public long Insert(Game game)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
        @"
        INSERT INTO games (name , year, globalSales)
        VALUES ($name, $year, $globalSales);
        SELECT last_insert_rowid();
        ";
        command.Parameters.AddWithValue("$name", game.name);
        command.Parameters.AddWithValue("$year", game.year);
        command.Parameters.AddWithValue("$globalSales", game.globalSales);
        long newId = (long)command.ExecuteScalar();
        connection.Close();
        return newId;
    }

    public List<Platform> GetGamePlatformsById(Game game)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT platforms.id, platforms.name  FROM games, platforms, connections WHERE games.id=$gameId AND games.id=connections.gameId AND platforms.id=connections.id";
        command.Parameters.AddWithValue("$gameId", game.id);
        SqliteDataReader reader = command.ExecuteReader();
        List<Platform> platforms = new List<Platform>();

        while (reader.Read())
        {
            Platform platform = new Platform();
            platform.id = long.Parse(reader.GetString(0));
            platform.name = reader.GetString(1);
            platforms.Add(platform);
        }

        reader.Close();
        connection.Close();
        return platforms;
    }

    public List<Game> GetAll()
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM games";
        SqliteDataReader reader = command.ExecuteReader();
        List<Game> games = new List<Game>();

        while (reader.Read())
        {
            Game game = new Game();
            game.id = long.Parse(reader.GetString(0));
            game.name = reader.GetString(1);
            game.year = int.Parse(reader.GetString(2));
            game.globalSales = double.Parse(reader.GetString(3));
            games.Add(game);
        }

        reader.Close();
        connection.Close();
        return games;

    }


    public void DeleteAll()
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM games";
        int nChanges = command.ExecuteNonQuery();
        connection.Close();
    }




}
