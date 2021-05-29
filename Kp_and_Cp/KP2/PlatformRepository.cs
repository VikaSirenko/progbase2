using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
public class PlatformRepository
{
    private SqliteConnection connection;
    public PlatformRepository(SqliteConnection connection)
    {
        this.connection = connection;
    }

    public long PlatformExists(string name)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM platforms WHERE name = $name";
        command.Parameters.AddWithValue("$name", name);
        SqliteDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            Platform platform = new Platform();
            platform.id = long.Parse(reader.GetString(0));
            platform.name = reader.GetString(1);
            return platform.id;

        }
        connection.Close();
        return 0;

    }
    public List<Platform> GetAll()
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM platforms";
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

    public long Insert(Platform platform)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
        @"
        INSERT INTO platforms (name )
        VALUES ($name);
        SELECT last_insert_rowid();
        ";
        command.Parameters.AddWithValue("$name", platform.name);
        long newId = (long)command.ExecuteScalar();
        connection.Close();
        return newId;
    }


   
    public void DeleteAll()
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM platforms";
        int nChanges = command.ExecuteNonQuery();
        connection.Close();
    }


}



