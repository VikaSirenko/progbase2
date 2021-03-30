using static System.Console;
using System.Text;
using System.IO;

public static class ProcessCommands
{
    public static void ProcessSets(ILogger logger)
    {
        ISetInt setA = new SetInt();
        ISetInt setB = new SetInt();
        bool do_command = true;

        while (do_command)
        {

            WriteLine($"\nWrite command:");
            string command = ReadLine();
            string[] subcommand = command.Split(' ');
            do_command = DoCommands(subcommand, logger, setA, setB);
        }
    }

    private static bool DoCommands(string[] subcommand, ILogger logger, ISetInt setA, ISetInt setB)
    {
        try
        {
            switch (subcommand[1])
            {
                case "add":
                    ProcessAdd(subcommand, logger, setA, setB);
                    return true;
                case "contains":
                    ProcessContains(subcommand, logger, setA, setB);
                    return true;
                case "remove":
                    ProcessRemove(subcommand, logger, setA, setB);
                    return true;
                case "clear":
                    ProcessClear(subcommand, logger, setA, setB);
                    return true;
                case "log":
                    ProcessLog(subcommand, logger, setA, setB);
                    return true;
                case "count":
                    ProcessCount(subcommand, logger, setA, setB);
                    return true;
                case "read":
                    ProcessRead(subcommand, logger, setA, setB);
                    return true;
                case "write":
                    ProcessWrite(subcommand, logger, setA, setB);
                    return true;
                default:
                    string command = string.Join(" ", subcommand);
                    logger.LogErrors($"Command [{command}] not found");
                    return true;
            }
        }

        catch
        {
            switch (subcommand[0])
            {
                case "IsSubsetOf":
                    ProcessIsSubsetOf(subcommand, logger, setA, setB);
                    return true;
                case "IntersectWith":
                    ProcessIntersectWith(subcommand, logger, setA, setB);
                    return true;
                case "exit":
                    return false;
                default:
                    string command = string.Join(" ", subcommand);
                    logger.LogErrors($"Command [{command}] not found");
                    return true;

            }
        }
    }

    private static void ProcessAdd(string[] subcommand, ILogger logger, ISetInt setA, ISetInt setB)
    {
        if (subcommand.Length != 3)
        {
            logger.LogErrors($"There should be [3] parts, but you have [{subcommand.Length}]");
        }

        else
        {
            ISetInt set = DetermineSet(subcommand, logger, setA, setB);

            if (set != default)
            {
                int value;
                bool isInt = int.TryParse(subcommand[2], out value);
                if (isInt)
                {
                    bool result = set.Add(value);
                    logger.Log($"The result of the ADD command: {result}");
                }
                else
                {
                    logger.LogErrors("Value entered incorrectly");

                }
            }
        }
    }


    private static void ProcessContains(string[] subcommand, ILogger logger, ISetInt setA, ISetInt setB)
    {
        if (subcommand.Length != 3)
        {
            logger.LogErrors($"There should be [3] parts, but you have [{subcommand.Length}]");
        }

        else
        {
            ISetInt set = DetermineSet(subcommand, logger, setA, setB);

            if (set != default)
            {
                int value;
                bool isInt = int.TryParse(subcommand[2], out value);

                if (isInt)
                {
                    bool result = set.Contains(value);
                    logger.Log($"The result of the CONTAINS command: {result}");
                }

                else
                {
                    logger.LogErrors("Value entered incorrectly");

                }
            }
        }
    }


    private static void ProcessRemove(string[] subcommand, ILogger logger, ISetInt setA, ISetInt setB)
    {
        if (subcommand.Length != 3)
        {
            logger.LogErrors($"There should be [3] parts, but you have [{subcommand.Length}]");
        }

        else
        {
            ISetInt set = DetermineSet(subcommand, logger, setA, setB);

            if (set != default)
            {
                int value;
                bool isInt = int.TryParse(subcommand[2], out value);

                if (isInt)
                {
                    bool result = set.Remove(value);
                    logger.Log($"The result of the REMOVE command: {result}");
                }

                else
                {
                    logger.LogErrors("Value entered incorrectly");

                }
            }
        }
    }


    private static void ProcessClear(string[] subcommand, ILogger logger, ISetInt setA, ISetInt setB)
    {
        if (subcommand.Length != 2)
        {
            logger.LogErrors($"There should be [2] parts, but you have [{subcommand.Length}]");
        }

        else
        {
            ISetInt set = DetermineSet(subcommand, logger, setA, setB);

            if (set != default)
            {
                set.Clear();
                logger.Log("The 'CLEAR' command was executed successfully");
            }
        }
    }


    private static void ProcessLog(string[] subcommand, ILogger logger, ISetInt setA, ISetInt setB)
    {
        if (subcommand.Length != 2)
        {
            logger.LogErrors($"There should be [2] parts, but you have [{subcommand.Length}]");
        }

        else
        {
            ISetInt set = DetermineSet(subcommand, logger, setA, setB);

            if (set != default)
            {
                int[] array = new int[set.GetCount()];
                set.CopyTo(array);
                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 0; i < array.Length; i++)
                {
                    stringBuilder.Append($"[{array[i].ToString()}], ");
                }

                string text = stringBuilder.ToString();
                logger.Log(text);
            }
        }
    }


    private static void ProcessCount(string[] subcommand, ILogger logger, ISetInt setA, ISetInt setB)
    {
        if (subcommand.Length != 2)
        {
            logger.LogErrors($"There should be [2] parts, but you have [{subcommand.Length}]");
        }

        else
        {
            ISetInt set = DetermineSet(subcommand, logger, setA, setB);

            if (set != default)
            {
                logger.Log($"The number of numbers in the set : {set.GetCount()}");
            }
        }
    }


    private static void ProcessRead(string[] subcommand, ILogger logger, ISetInt setA, ISetInt setB)
    {
        if (subcommand.Length != 3)
        {
            logger.LogErrors($"There should be [3] parts, but you have [{subcommand.Length}]");
        }

        else
        {
            ISetInt set = DetermineSet(subcommand, logger, setA, setB);

            if (set != default)
            {
                ISetInt set1 = ReadSet(subcommand[2], logger);
                int[] array = new int[set1.GetCount()];
                set1.CopyTo(array);

                for (int i = 0; i < array.Length; i++)
                {
                    set.Add(array[i]);
                }

                logger.Log("The 'READ' command was executed successfully");
            }
        }
    }



    private static void ProcessWrite(string[] subcommand, ILogger logger, ISetInt setA, ISetInt setB)
    {
        if (subcommand.Length != 3)
        {
            logger.LogErrors($"There should be [3] parts, but you have [{subcommand.Length}]");
        }

        else
        {
            ISetInt set = DetermineSet(subcommand, logger, setA, setB);

            if (set != default)
            {
                WriteSet(subcommand[2], set);
            }
            logger.Log("The 'WRITE' command was executed successfully");

        }
    }

    private static void ProcessIsSubsetOf(string[] subcommand, ILogger logger, ISetInt setA, ISetInt setB)
    {
        if (subcommand.Length != 1)
        {
            logger.LogErrors($"There should be [1] part, but you have [{subcommand.Length}]");
        }

        else
        {
            bool aIsSubset = setA.IsSubsetOf(setB);
            logger.Log($"Set A is a subset of B: {aIsSubset}");
        
        }
    }

    private static void ProcessIntersectWith(string[] subcommand, ILogger logger, ISetInt setA, ISetInt setB)
    {
        if (subcommand.Length != 1)
        {
            logger.LogErrors($"There should be [1] part, but you have [{subcommand.Length}]");
        }

        else
        {
            setA.IntersectWith(setB);
            logger.Log("The 'IntersectWith' command over the set A was executed successfully");
            
        }
    }


    private static ISetInt DetermineSet(string[] subcommand, ILogger logger, ISetInt setA, ISetInt setB)
    {
        switch (subcommand[0])
        {
            case "a":
                return setA;
            case "b":
                return setB;
            default:
                logger.LogErrors($"The set [{subcommand[0]}] does not exist");
                return default;

        }
    }


    private static ISetInt ReadSet(string filePath, ILogger logger)
    {
        ISetInt set = new SetInt();

        if (File.Exists(filePath))
        {
            string line = File.ReadAllText(filePath);
            string[] numbers = line.Split();

            for (int i = 0; i < numbers.Length; i++)
            {
                int num;
                bool isNum = int.TryParse(numbers[i], out num);

                if (isNum)
                {
                    set.Add(num);
                }

                else
                {
                    logger.LogErrors("Value entered incorrectly");
                }
            }
        }

        else
        {
            logger.LogErrors($"File  {filePath} does not exist");

        }

        return set;
    }


    private static void WriteSet(string filePath, ISetInt set)
    {
        int[] array = new int[set.GetCount()];
        set.CopyTo(array);
        StreamWriter writer = new StreamWriter(filePath);

        for (int i = 0; i < array.Length; i++)
        {
            writer.Write($"{array[i]} ");
        }
        writer.Close();
    }
}

