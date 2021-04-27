using System;

namespace lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            DataProcessor processor = new DataProcessor();
            try
            {
                CommandProcessor.Run(processor);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                CommandProcessor.Run(processor);

            }
        }
    }
}
