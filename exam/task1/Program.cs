using System;
using System.IO;
using static System.Console;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args[0] == "gen_num")
            {
                ProcessGeneretionNumbers(args);

            }
            else if (args[0] == "num")
            {
                ProcessFindLargestEvenNum(args);
            }
            else if (args[0] == "num_uni")
            {
                ReadSpecificNumbersFromFile(args);
            }
            else
            {
                Error.WriteLine("The command is set incorrectly");
            }
        }
        catch (Exception ex)
        {
            Error.WriteLine(ex.Message.ToString());
        }


    }

    static void ProcessGeneretionNumbers(string[] args)
    {
        if (args.Length != 5)
        {
            throw new Exception("The command is set incorrectly");
        }
        int a;
        int b;
        int n;

        bool isNumA = int.TryParse(args[2], out a);
        bool isNumB = int.TryParse(args[3], out b);
        bool isNumN = int.TryParse(args[4], out n);
        if (isNumA && isNumB && isNumN)
        {
            Random random = new Random();
            for (int i = 0; i < n; i++)
            {
                int row = random.Next(a, b);
                string text = File.ReadAllText(args[1]);
                text += Environment.NewLine + row;
                File.WriteAllText(args[1], text);
            }

        }
        else
        {
            throw new Exception("The interval and number of numbers are set incorrectly");
        }
    }

    static void ProcessFindLargestEvenNum(string[] args)
    {
        if (args.Length != 2)
        {
            throw new Exception("The command is set incorrectly");
        }
        else if (!File.Exists(args[1]))
        {
            throw new Exception("Number file does not exist");
        }
        string[] rows = File.ReadAllLines(args[1]);
        int maxNum = default;
        for (int i = 0; i < rows.Length; i++)
        {
            int row;
            bool isNum = int.TryParse(rows[i], out row);
            if (isNum)
            {
                if (row % 2 == 0 && row > maxNum)
                {
                    maxNum = row;
                }
            }
            else
            {
                throw new Exception("impossible to parse number");
            }
        }
        Write("The largest even number is:  " + maxNum);


    }

    static void ReadSpecificNumbersFromFile(string[] args)
    {
        if (args.Length != 3)
        {
            throw new Exception("The command is set incorrectly");

        }
        else if (!File.Exists(args[1]))
        {
            throw new Exception("Number file does not exist");
        }
        List<int> listOfNum = new List<int>();
        string[] rows = File.ReadAllLines(args[1]);
        for (int i = 0; i < rows.Length; i++)
        {
            int row;
            bool isNum = int.TryParse(rows[i], out row);
            if (isNum)
            {
                if (!listOfNum.Contains(row))
                {
                    listOfNum.Add(row);
                }
            }
            else
            {
                throw new Exception("impossible to parse number");
            }
        }
        WriteDataInFile(listOfNum, args[2]);

    }

    static void WriteDataInFile(List<int> listofNum, string pathToFile)
    {
        for (int i = 0; i < listofNum.Count; i++)
        {
            int row = listofNum[i];
            string text = File.ReadAllText(pathToFile);
            text += Environment.NewLine + row;
            File.WriteAllText(pathToFile, text);
        }
    }
}

