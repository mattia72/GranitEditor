using GranitXMLEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GranitXMLEditorTests
{
  public class TestConstants
  {
    public static string TransactionXml = @"  
      <Transaction>
       <Originator> <Account> <AccountNumber>111111112222222233333333</AccountNumber> </Account> </Originator>
       <Beneficiary> <Name>Gipsz Jakab</Name> <Account> <AccountNumber>333333334444444455555555</AccountNumber> </Account> </Beneficiary>
       <Amount Currency = ""HUF"" >1000.00</Amount>
       <RequestedExecutionDate>2016-12-02</RequestedExecutionDate>
       <RemittanceInfo> <Text>Utólagos elszámolásra</Text> </RemittanceInfo>
      </Transaction>
";
    public static string HUFTransactionXml = @"
<HUFTransactions>
      <Transaction id=""1"" is_active=""true"">
       <Originator> <Account> <AccountNumber>111111112222222233333333</AccountNumber> </Account> </Originator>
       <Beneficiary> <Name>Gipsz Jakab</Name> <Account> <AccountNumber>333333334444444455555555</AccountNumber> </Account> </Beneficiary>
       <Amount Currency = ""HUF"" >1000.00</Amount>
       <RequestedExecutionDate>2016-12-02</RequestedExecutionDate>
       <RemittanceInfo> <Text>Utólagos elszámolásra</Text> </RemittanceInfo>
      </Transaction>
</HUFTransactions>
";
  }
}
