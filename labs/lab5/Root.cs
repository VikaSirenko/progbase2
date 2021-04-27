using System.Collections.Generic;
using System.Xml.Serialization;

namespace lab5
{
    [XmlRoot("table")]
    public class Root
    {
        [XmlElement("T")]
        public List<Customer> customers;
    }
}