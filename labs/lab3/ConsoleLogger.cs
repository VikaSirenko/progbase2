using static System.Console;
public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        WriteLine($"{message}"); 
    }

    public void LogErrors(string errorMessage)
    {
         WriteLine($"{errorMessage}");
    }
}
