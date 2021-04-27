using System;
using static System.Console;
using System.Collections.Generic;


namespace lab5
{
    public static class CommandProcessor
    {

        public static void Run(DataProcessor processor)
        {
            bool run = true;
            while (run)
            {
                WriteLine();
                WriteLine("Enter the command");
                string inputCommand = ReadLine();
                string[] userCommand = inputCommand.Split(" ");
                run = ParseCommandLine(userCommand, processor);
            }

        }


        private static bool ParseCommandLine(string[] userCommand, DataProcessor processor)
        {
            if (userCommand.Length < 1)
            {
                throw new ArgumentException($"Not enough command line arguments. Expected more than 1, got [{userCommand.Length}] ");
            }
            ValidateCommand(userCommand[0]);

            switch (userCommand[0])
            {
                case "load":
                    ProcessLoad(userCommand, processor);
                    break;
                case "print":
                    ProcessPrint(userCommand, processor);
                    break;
                case "save":
                    ProcessSave(userCommand, processor);
                    break;
                case "export":
                    ProcessExport(userCommand, processor);
                    break;
                case "image":
                    ProcessImage(userCommand, processor);
                    break;
                case "balances":
                    ProcessBalances(userCommand, processor);
                    break;
                case "names":
                    ProcessNames(userCommand, processor);
                    break;
                case "comments":
                    ProcessComments(userCommand, processor);
                    break;
                case "exit":
                    return false;

            }
            return true;

        }

        private static void ValidateCommandLineLength(int currentLength, int expectedLength)
        {
            if (currentLength != expectedLength)
            {
                throw new ArgumentException($"Not enough command line arguments. Expected [{expectedLength}], got [{currentLength}] ");
            }

        }


        private static void ValidateCommand(string command)
        {
            string[] supportedCommands = new string[] { "load", "print", "save", "export", "image", "balances", "names", "comments", "exit" };
            for (int i = 0; i < supportedCommands.Length; i++)
            {
                if (supportedCommands[i] == command)
                {
                    return;
                }

            }
            throw new ArgumentException($"Not supported command: [{command}]");
        }

        private static void ValidateInputFile(string file)
        {
            if (System.IO.File.Exists(file) == false)
            {
                throw new ArgumentException($"Input file does not exist : [{file}]");
            }
        }

        private static int ParseNumber(string num)
        {
            int result;
            bool isNum = int.TryParse(num, out result);
            if (isNum == false)
            {
                throw new FormatException($"The command input value [{num}] cannot be parsed");
            }

            return int.Parse(num);
        }

        private static void CheckForData(Root root)
        {
            if (root != null)
            {
                return;
            }
            throw new Exception("There is no data to execute the command. Download data first.");

        }



        private static void ProcessLoad(string[] userCommand, DataProcessor processor)
        {
            ValidateCommandLineLength(userCommand.Length, 2);
            ValidateInputFile(userCommand[1]);
            processor.root = Serializer.DoDeserialization(userCommand[1]);
        }


        private static void ProcessPrint(string[] userCommand, DataProcessor processor)
        {
            ValidateCommandLineLength(userCommand.Length, 2);
            int pageNum = ParseNumber(userCommand[1]);
            CheckForData(processor.root);
            int totalPages = processor.GetCountOfTotalPages();
            WriteLine($"Total number of pages:{totalPages}");
            if (totalPages < pageNum || pageNum < 1)
            {
                throw new Exception($"Page number [{pageNum}] does not exist");
            }
            List<Customer> customersFromPage = processor.GetPage(pageNum);
            processor.Output(customersFromPage);

        }

        private static void ProcessSave(string[] userCommand, DataProcessor processor)
        {
            ValidateCommandLineLength(userCommand.Length, 2);
            CheckForData(processor.root);
            Serializer.DoSerialization(processor.root.customers, userCommand[1]);
        }

        private static void ProcessExport(string[] userCommand, DataProcessor processor)
        {
            ValidateCommandLineLength(userCommand.Length, 3);
            int N = ParseNumber(userCommand[1]);
            CheckForData(processor.root);
            if (N < 1 || N > processor.root.customers.Count)
            {
                throw new Exception($"There is no such information about customers.The most information you can get is [{processor.root.customers.Count}]");
            }
            List<Customer> customersWithGreatestValue = processor.FindWithGreatestValue(N);
            Serializer.DoSerialization(customersWithGreatestValue, userCommand[2]);

        }
        private static void ProcessImage(string[] userCommand, DataProcessor processor)
        {
            ValidateCommandLineLength(userCommand.Length, 2);
            CheckForData(processor.root);
            Image image = new Image();
            Image.MarketSegmentation segmentation = image.CountCustomersInCategory(processor.root.customers);
            ScottPlot.Plot plt = image.FormBarGraph(segmentation);
            plt.SaveFig(userCommand[1]);

        }

        private static void ProcessBalances(string[] userCommand, DataProcessor processor)
        {
            ValidateCommandLineLength(userCommand.Length, 1);
            CheckForData(processor.root);
            processor.DoSort(processor.root.customers);
            processor.Output(processor.root.customers);

        }

        private static void ProcessNames(string[] userCommand, DataProcessor processor)
        {
            ValidateCommandLineLength(userCommand.Length, 2);
            int nationKey = ParseNumber(userCommand[1]);
            CheckForData(processor.root);
            List<string> names = processor.GetNamesList(nationKey);
            if (names.Count == 0)
            {
                WriteLine("There are no customers with such NATION KEY");
            }
            else
            {
                for (int i = 0; i < names.Count; i++)
                {
                    WriteLine($"{names[i]}");
                }

            }
        }

        private static void ProcessComments(string[] userCommand, DataProcessor processor)
        {
            ValidateCommandLineLength(userCommand.Length, 1);
            CheckForData(processor.root);
            List<string> comments = processor.GetCommentsList();
            for (int i = 0; i < comments.Count; i++)
            {
                WriteLine($"{comments[i]}");
            }

        }

    }
}