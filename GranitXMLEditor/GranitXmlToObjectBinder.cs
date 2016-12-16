using System.IO;
using System.Xml;
using System.Xml.Serialization;
using GranitXMLEditor;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Linq;
using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace GranitXMLEditor
{
    internal class GranitXmlToObjectBinder
    {
        internal HUFTransactions HUFTransactions { get; private set; }
        internal HUFTransactionAdapter HUFTransactionAdapter { get; private set; }
        internal XDocument GranitXDocument { get; private set; }

        private GranitXmlToObjectBinder()
        {
            GranitXDocument = new XDocument();
        }

        public GranitXmlToObjectBinder(HUFTransactions hufTransactions) : this()
        {
            HUFTransactions = hufTransactions;
            HUFTransactionAdapter = new HUFTransactionAdapter(HUFTransactions, GranitXDocument);
            UpdateGranitXDocument();
        }

        public GranitXmlToObjectBinder(string xmlFilePath) : this()
        {
            LoadObjectFromFile(xmlFilePath);
            UpdateGranitXDocument();
            HUFTransactionAdapter = new HUFTransactionAdapter(HUFTransactions, GranitXDocument);
        }

        public void UpdateGranitXDocument()
        {
            XDocument xml = new XDocument();
            using (var writer = xml.CreateWriter())
            {
                var ser = new XmlSerializer(HUFTransactions.GetType());
                ser.Serialize(writer, HUFTransactions);
            }

            if (GranitXDocument.Elements().Count() == 0)
                GranitXDocument = xml;
            else
            {
                // merge xmls
                //GranitXmlDoc.
            }


        }

        public void LoadObjectFromFile(string xmlFilePath)
        {
            var ser = new XmlSerializer(typeof(HUFTransactions));
            var xml = XDocument.Load(xmlFilePath);
            HUFTransactions = (HUFTransactions)ser.Deserialize(xml.CreateReader());
        }

        public void SaveToFile(string xmlFilePath)
        {
            GranitXDocument.Save(xmlFilePath);
        }

        public void Sort(string columnText )
        {
            var sorted = GranitXDocument.Descendants(Constants.Transaction).OrderBy(x => x.Element("Amount").Value);
        }
    }
}