using GranitXml;
using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GranitEditor
{
  internal class TransactionXElementParser : XElement
  {
    public XElement ParsedElement { get; set; }

    public TransactionXElementParser(): base(GranitXml.Constants.Transaction)
    {
      ParsedElement = Parse(DefaultTransactionXml);
    }

    public TransactionXElementParser(TransactionAdapter ta): base(GranitXml.Constants.Transaction)
    {
      ParsedElement = Parse(ta.Transaction);
    }

    private string DefaultTransactionXml = @"  
      <Transaction>
       <Originator> <Account> <AccountNumber>000000000000000000000000</AccountNumber> </Account> </Originator>
       <Beneficiary> <Name></Name> <Account> <AccountNumber>000000000000000000000000</AccountNumber> </Account> </Beneficiary>
       <Amount Currency = ""HUF"" >1.00</Amount>
       <RequestedExecutionDate>" + DateTime.Now.ToString(GranitXml.Constants.DateFormat) + @"</RequestedExecutionDate>
       <RemittanceInfo> <Text></Text> </RemittanceInfo>
      </Transaction> ";

    private XElement Parse(Transaction ta)
    {
      XDocument xml = new XDocument();

      using (var writer = xml.CreateWriter())
      {
        var ser = new XmlSerializer(ta.GetType());
        ser.Serialize(writer, ta);
      }
      return xml.Root;
    }
  }
}                                                                     