using System;
using static System.Console;
using System.Collections.Generic;


namespace lab5
{
    public static class CommandProcessor
    {

        public static void Run()
        {
            bool run = true;
            List<Customer> customers = new List<Customer>();
            while (run)
            {
                WriteLine();
                WriteLine("Enter the command");
                string inputCommand = ReadLine();
                string[] userCommand = inputCommand.Split(" ");
                try
                {
                    ValidateCommand(userCommand[0]);
                    switch (userCommand[0])
                    {
                        case "load":
                            customers = ProcessLoad(userCommand);
                            break;
                        case "print":
                            ProcessPrint(userCommand, customers);
                            break;
                        case "save":
                            ProcessSave(userCommand, customers);
                            break;
                        case "export":
                            ProcessExport(userCommand, customers);
                            break;
                        case "image":
                            ProcessImage(userCommand, customers);
                            break;
                        case "balances":
                            ProcessBalances(userCommand, customers);
                            break;
                        case "names":
                            ProcessNames(userCommand, customers);
                            break;
                        case "comments":
                            ProcessComments(userCommand, customers);
                            break;
                        case "exit":
                            run = false;
                            break;

                    }

                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);

                }
            }

        }


        private static void ValidateCommandLineLength(int currentLength, int expectedLength)
        {
            if (currentLength != expectedLength)
            {
                throw new ArgumentException($"The number of command line arguments is incorrect. Expected [{expectedLength}], got [{currentLength}] ");
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

        private static void CheckForData(List<Customer> customers)
        {
            if (customers.Count != 0)
            {
                return;
            }
            throw new Exception("There is no data to execute the command. Download data first.");

        }



        private static List<Customer> ProcessLoad(string[] userCommand)
        {
            ValidateCommandLineLength(userCommand.Length, 2);
            ValidateInputFile(userCommand[1]);
            List<Customer> customers = Serializer.DoDeserialization(userCommand[1]);
            WriteLine("Downloaded.");
            return customers;
        }


        private static void ProcessPrint(string[] userCommand, List<Customer> customers)
        {
            ValidateCommandLineLength(userCommand.Length, 2);
            int pageNum = ParseNumber(userCommand[1]);
            CheckForData(customers);
            int totalPages = DataProcessor.GetCountOfTotalPages(customers.Count);
            WriteLine($"Total number of pages:{totalPages}");
            if (totalPages < pageNum || pageNum < 1)
            {
                throw new Exception($"Page number [{pageNum}] does not exist");
            }
            List<Customer> customersFromPage = DataProcessor.GetPage(pageNum, customers);
            Output(customersFromPage);

        }

        private static void ProcessSave(string[] userCommand, List<Customer> customers)
        {
            ValidateCommandLineLength(userCommand.Length, 2);
            CheckForData(customers);
            Serializer.DoSerialization(customers, userCommand[1]);
            WriteLine("Saved in: " + userCommand[1]);
        }

        private static void ProcessExport(string[] userCommand, List<Customer> customers)
        {
            ValidateCommandLineLength(userCommand.Length, 3);
            int N = ParseNumber(userCommand[1]);
            CheckForData(customers);
            if (N < 1 || N > customers.Count)
            {
                throw new Exception($"There is no such information about customers.The most information you can get is [{customers.Count}]");
            }
            List<Customer> customersWithGreatestValue = DataProcessor.FindWithGreatestValue(N, customers);
            Serializer.DoSerialization(customersWithGreatestValue, userCommand[2]);
            WriteLine("Exported in:" + userCommand[2]);

        }
        private static void ProcessImage(string[] userCommand, List<Customer> customers)
        {
            ValidateCommandLineLength(userCommand.Length, 2);
            CheckForData(customers);
            Image.MarketSegmentation segmentation = Image.CountCustomersInCategory(customers);
            ScottPlot.Plot plt = Image.FormBarGraph(segmentation);
            plt.SaveFig(userCommand[1]);
            WriteLine("Image created");

        }

        private static void ProcessBalances(string[] userCommand, List<Customer> customers)
        {
            ValidateCommandLineLength(userCommand.Length, 1);
            CheckForData(customers);
            DataProcessor.DoSort(customers);
            Output(customers);

        }

        private static void ProcessNames(string[] userCommand, List<Customer> customers)
        {
            ValidateCommandLineLength(userCommand.Length, 2);
            int nationKey = ParseNumber(userCommand[1]);
            CheckForData(customers);
            List<string> names = DataProcessor.GetNamesList(nationKey, customers);
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

        private static void ProcessComments(string[] userCommand, List<Customer> customers)
        {
            ValidateCommandLineLength(userCommand.Length, 1);
            CheckForData(customers);
            List<string> comments = DataProcessor.GetCommentsList(customers);
            for (int i = 0; i < comments.Count; i++)
            {
                WriteLine($"{comments[i]}");
            }

        }

        private static void Output(List<Customer> customers)
        {
            for (int i = 0; i < customers.Count; i++)
            {
                Customer customer = customers[i];
                Console.WriteLine(
                @$"C_CUSTKEY: {customer.customerKey}
                C_NAME: {customer.name}
                C_ADDRESS: {customer.address}
                C_NATIONKEY: {customer.nationKey}
                C_PHONE: {customer.phoneNumber}
                C_ACCTBAL: {customer.accountBalanceRebate}
                C_MKTSEGMENT: {customer.marketSegmentation}
                C_COMMENT: {customer.comment}");
                Console.WriteLine();
            }

        }


    }
}