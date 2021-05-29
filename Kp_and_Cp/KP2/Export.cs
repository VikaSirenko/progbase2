using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

public class Export
{
    public static void DoExportOfPlatforms(List<Platform> platforms, string outputFile)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Platform>));
        StreamWriter output = new StreamWriter(outputFile);
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.NewLineHandling = NewLineHandling.Entitize;
        XmlWriter writer = XmlWriter.Create(output, settings);
        serializer.Serialize(output, platforms);
        output.Close();
    }

    public static void DoExportOfGame(Game game, string outputFile)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Game));
        StreamWriter output = new StreamWriter(outputFile);
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.NewLineHandling = NewLineHandling.Entitize;
        XmlWriter writer = XmlWriter.Create(output, settings);
        serializer.Serialize(output, game);
        output.Close();
    }

}
