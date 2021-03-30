using static System.Console;


class Program
{
    static void Main(string[] args)
    {
        if(args.Length== 0)
        {
             ProcessCommands.ProcessSets(new ConsoleLogger());
        }

        else if(args.Length==1 && args[0]=="console")
        {
            ProcessCommands.ProcessSets(new ConsoleLogger());

        }

        else if(args.Length==2 && args[0]=="csv")
        {
            ProcessCommands.ProcessSets(new CsvFileLogger(args[1]));
        }

        else
        {
            WriteLine("Select the correct logger");
        }
    }
}

