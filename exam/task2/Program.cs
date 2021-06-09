using System;
using static System.Console;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;


class Program
{
    static void Main(string[] args)
    {
        try
        {
            WriteLine("input command");
            string command = ReadLine();
            string[] commandParts = command.Split("");

            if (commandParts[0] == "gen_db")
            {
                ProcessGeneretionDB(commandParts);
            }
            else if (commandParts[0] == "get_name")
            {
                ProcessGetName(commandParts);
            }
            else if (commandParts[0] == "merge_csv")
            {
                ProcessMergeCSV(commandParts);
            }
            else
            {
                Error.WriteLine("Command not found");
            }
        }
        catch (Exception ex)
        {
            Error.WriteLine(ex.Message.ToString());
        }


    }
    static void ProcessGeneretionDB(string[] commandParts)
    {
        if (commandParts.Length != 5)
        {
            throw new Exception("the command is set incorrectly");
        }
        else if (!File.Exists(commandParts[1]))
        {
            throw new Exception("The path to the file is incorrect");
        }
        else if (Directory.Exists(commandParts[2]))
        {
            throw new Exception("The path to the directory is incorrect");
        }
        int n;
        int m;

        bool isNumN = int.TryParse(commandParts[3], out n);
        bool isNumM = int.TryParse(commandParts[4], out m);
        if (isNumM && isNumN)
        {
            while (n > 0)
            {
                int i = m;
                while (i > 0)
                {

                    string path = commandParts[2] + "/" + i + "BD";
                    File.Copy("./BD", path);
                    SqliteConnection connection = new SqliteConnection($"Data Source={path}");
                    string line = FindRandomLineInFile(commandParts[1], m);
                    string[] dataParts = line.Split(",");
                    Award award = new Award();
                    award.year = int.Parse(dataParts[0]);
                    award.ceremony = dataParts[1];
                    award.award = dataParts[2];
                    award.winner = dataParts[3];
                    award.name = dataParts[4];
                    award.film = dataParts[5];
                    AwardsRepository awardsRepository = new AwardsRepository(connection);
                    if (!awardsRepository.AwardExists(award))
                    {
                        awardsRepository.Insert(award);
                        i--;
                    }
                }

            }
        }



    }

    private static string FindRandomLineInFile(string path, int num)
    {
        Random random = new Random();
        int numberOfLine = random.Next(1, num * 100);
        StreamReader finder = new StreamReader(path);
        string search_line = "";

        while (numberOfLine > 0)
        {
            search_line = finder.ReadLine();
            numberOfLine--;
        }

        finder.Close();
        return search_line;
    }

    static void ProcessGetName(string[] commandParts)
    {
        if (commandParts.Length != 3)
        {
            throw new Exception("the command is set incorrectly");
        }
        else if (Directory.Exists(commandParts[1]))
        {
            throw new Exception("The path to the directory is incorrect");
        }
        string[] files = Directory.GetFiles(commandParts[1]);

        for (int i = 0; i < files.Length; i++)
        {
            try
            {
                if (files[i].EndsWith("BD"))
                {
                    SqliteConnection connection = new SqliteConnection($"Data Source={files[i]}");
                    AwardsRepository awardsRepository = new AwardsRepository(connection);
                    List<Award> awardsList = awardsRepository.ParticipantExists(commandParts[2]);
                    Print(awardsList);

                }

            }
            catch (Exception ex)
            {
                Error.WriteLine(ex.Message.ToString());

            }

        }



    }

    static void Print(List<Award> awardList)
    {
        if (awardList.Count != 0)
        {
            for (int i = 0; i < awardList.Count; i++)
            {
                WriteLine($"Year: {awardList[i].year} \nCeremony: {awardList[i].ceremony} \nAward: {awardList[i].award} \n Winner: {awardList[i].winner} \nName: {awardList[i].name}  \nFilm: {awardList[i].film}\n");
                WriteLine();
            }
        }

    }

    static void ProcessMergeCSV(string[] commandParts)
    {
        if (commandParts.Length != 3)
        {
            throw new Exception("the command is set incorrectly");
        }
        else if (Directory.Exists(commandParts[1]))
        {
            throw new Exception("The path to the directory is incorrect");
        }
        string[] files = Directory.GetFiles(commandParts[1]);

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].EndsWith("BD"))
            {
                SqliteConnection connection = new SqliteConnection($"Data Source={files[i]}");
                AwardsRepository awardsRepository = new AwardsRepository(connection);
                List<Award> awardsList = awardsRepository.GetAll();
                WriteInCsvFile(commandParts[2], awardsList);

            }


        }
    }

    static void WriteInCsvFile(string path, List<Award> awardList)
    {
        if (awardList.Count != 0)
        {
            for (int i = 0; i < awardList.Count; i++)
            {
                string line = awardList[i].year + "," + awardList[i].ceremony + "," + awardList[i].award + "," + awardList[i].winner + "," + awardList[i].name + "," + awardList[i].film;
                string text = File.ReadAllText(path);
                text += Environment.NewLine + line;
                File.WriteAllText(path, text);
            }

        }

    }
}

