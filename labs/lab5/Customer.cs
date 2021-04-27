using System.Xml.Serialization;

namespace lab5
{
    public class Customer
    {
        [XmlElement("C_CUSTKEY")]
        public int customerKey;

        [XmlElement("C_NAME")]
        public string name;

        [XmlElement("C_ADDRESS")]
        public string address;


        [XmlElement("C_NATIONKEY")]
        public int nationKey;

        [XmlElement("C_PHONE")]
        public string phoneNumber;

        [XmlElement("C_ACCTBAL")]
        public double accountBalanceRebate;

        [XmlElement("C_MKTSEGMENT")]
        public string marketSegmentation;

        [XmlElement("C_COMMENT")]
        public string comment;

    }
}