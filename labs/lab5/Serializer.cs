using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace lab5
{
    public static class Serializer
    {

        public static void DoSerialization(List<Customer> customers, string outputFile)
        {
            Root root = new Root
            {
                customers = customers,
            };
            XmlSerializer serializer = new XmlSerializer(typeof(Root));
            StreamWriter output = new StreamWriter(outputFile);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineHandling = NewLineHandling.Entitize;
            XmlWriter writer = XmlWriter.Create(output, settings);
            serializer.Serialize(output, root);
            output.Close();

        }

        public static Root DoDeserialization(string inputFile)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Root));
            StreamReader reader = new StreamReader(inputFile);
            Root customers = (Root)serializer.Deserialize(reader);
            reader.Close();
            return customers;
        }


    }
}