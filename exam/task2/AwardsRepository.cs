using Microsoft.Data.Sqlite;
using System.Collections.Generic;
public class AwardsRepository
{

    private SqliteConnection connection;
    public AwardsRepository(SqliteConnection connection)
    {
        this.connection = connection;
    }

    public long Insert(Award award)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
        @"
                INSERT INTO awards (year, ceremony, award, winner, name , film )
                VALUES ($year, $ceremony, $award, $winner, $name , $film);
                SELECT last_insert_rowid();
                ";
        command.Parameters.AddWithValue("year", award.year);
        command.Parameters.AddWithValue("$ceremony", award.ceremony);
        command.Parameters.AddWithValue("$award", award.award);
        command.Parameters.AddWithValue("$winner", award.winner);
        command.Parameters.AddWithValue("$name", award.name);
        command.Parameters.AddWithValue("$film", award.film);
        long newId = (long)command.ExecuteScalar();
        connection.Close();
        return newId;
    }


    public bool AwardExists(Award award)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM awards WHERE year=$year AND ceremony=$ceremony AND award=$award AND winner=$winner ANd name=$name AND film=$film";
        command.Parameters.AddWithValue("year", award.year);
        command.Parameters.AddWithValue("$ceremony", award.ceremony);
        command.Parameters.AddWithValue("$award", award.award);
        command.Parameters.AddWithValue("$winner", award.winner);
        command.Parameters.AddWithValue("$name", award.name);
        command.Parameters.AddWithValue("$film", award.film);
        SqliteDataReader reader = command.ExecuteReader();
        bool result = reader.Read();
        connection.Close();
        return result;
    }

    public List<Award> ParticipantExists(string name)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.Parameters.AddWithValue("$name", name);
        SqliteDataReader reader = command.ExecuteReader();
        List<Award> awardsList = new List<Award>();
        while (reader.Read())
        {
            Award award = ParseAward(reader);
            awardsList.Add(award);
        }
        reader.Close();
        connection.Close();
        return awardsList;

    }

    private Award ParseAward(SqliteDataReader reader)
    {
        Award award = new Award();
        award.id = long.Parse(reader.GetString(0));
        award.year = int.Parse(reader.GetString(1));
        award.ceremony = reader.GetString(2);
        award.award = reader.GetString(3);
        award.winner = reader.GetString(4);
        award.name = reader.GetString(5);
        award.film = reader.GetString(6);
        return award;
    }

    public List<Award> GetAll()
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM awards";
        SqliteDataReader reader = command.ExecuteReader();
        List<Award> awardList = new List<Award>();
        while (reader.Read())
        {
            Award award = ParseAward(reader);
            awardList.Add(award);
        }
        reader.Close();
        connection.Close();
        return awardList;
    }

}
