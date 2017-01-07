using System;
using System.Xml.Linq;

namespace GranitXMLEditor
{
  internal class TransactionXElement : XElement
  {

    public TransactionXElement():
      base(Constants.Transaction)
    {
      ParsedElement = Parse(TransactionXml);
    }

    public XElement ParsedElement { get; set; }

    private string TransactionXml = @"  
      <Transaction>
       <Originator> <Account> <AccountNumber>000000000000000000000000</AccountNumber> </Account> </Originator>
       <Beneficiary> <Name></Name> <Account> <AccountNumber>000000000000000000000000</AccountNumber> </Account> </Beneficiary>
       <Amount Currency = ""HUF"" >1.00</Amount>
       <RequestedExecutionDate>" + DateTime.Now.ToString(Constants.DateFormat) + @"</RequestedExecutionDate>
       <RemittanceInfo> <Text>Utólagos elszámolásra</Text> </RemittanceInfo>
      </Transaction> ";
  }
}                                                                     