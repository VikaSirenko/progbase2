using Microsoft.Data.Sqlite;

public class ConnectionRepository
{
    private SqliteConnection connection;
    public ConnectionRepository(SqliteConnection connection)
    {
        this.connection = connection;
    }

    public long Insert(Connection gameAndPlatformconnection)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
        @"
        INSERT INTO connections (gameId, platformId )
        VALUES ($gameId, $platformId);
        SELECT last_insert_rowid();
        ";
        command.Parameters.AddWithValue("$gameId", gameAndPlatformconnection.gameId);
        command.Parameters.AddWithValue("$platformId", gameAndPlatformconnection.platformId);
        long newId = (long)command.ExecuteScalar();
        connection.Close();
        return newId;
    }


    public void DeleteAll()
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM connections";
        int nChanges = command.ExecuteNonQuery();
        connection.Close();
    }
}
