using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Xml2CSharp;

namespace GranitXMLTemplate
{
    internal class GranitXmlToObject
    {
        internal HUFTransactions HUFTransactions { get; private set; }
        internal XmlDocument GranitXmlDoc{ get; private set; }

        public HUFTransactionAdapter HUFTransactionAdapter { get { return new HUFTransactionAdapter(HUFTransactions); } }

        private GranitXmlToObject()
        {
            GranitXmlDoc = new XmlDocument();
        }

        public GranitXmlToObject( HUFTransactions hufTransactions )
            : this()
        {
            HUFTransactions = hufTransactions;
            GenerateXmlDocument();
        }

        public GranitXmlToObject( string xmlFilePath )
            : this()
        {
            ReadFromFile(xmlFilePath);
        }

        public void GenerateXmlDocument()
        {
            var nav = GranitXmlDoc.CreateNavigator();
            using (var writer = nav.AppendChild())
            {
                var ser = new XmlSerializer(HUFTransactions.GetType());
                ser.Serialize(writer, HUFTransactions);
            }
        }

        public void ReadFromFile(string xmlFilePath)
        {
            GranitXmlDoc.Load(xmlFilePath);
            var serializer = new XmlSerializer(typeof(HUFTransactions));

            using (var reader = new XmlNodeReader(GranitXmlDoc))
            {
                HUFTransactions = (HUFTransactions)serializer.Deserialize(reader);
            }
        }
    }
}