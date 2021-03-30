using System.IO;
using System;
public class CsvFileLogger : ILogger
{
    private string filePath;
    public CsvFileLogger(string filePath)
    {
        this.filePath=filePath;
        File.WriteAllText(this.filePath , "timestamp,type,message");
        
    }
    
    public void Log(string message)
    {
        message=message.Replace(",", " ");
        string text= File.ReadAllText(this.filePath);
        string row = $"{DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss.fff")},LOG,{message}";
        text+= Environment.NewLine+ row;
        File.WriteAllText(this.filePath, text);
    }

    public void LogErrors(string errorMessage)
    {
        errorMessage=errorMessage.Replace(",", " ");
        string text= File.ReadAllText(this.filePath);
        string row = $"{DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss.fff")},ERROR,{errorMessage}";
        text+= Environment.NewLine+ row;
        File.WriteAllText(this.filePath, text);
    }
}
