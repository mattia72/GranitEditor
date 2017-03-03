﻿namespace GranitXMLEditorTests
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

    public static string TransactionXElem1 = @"
      <Transaction id=""1"" is_selected=""true"">
       <Originator> <Account> <AccountNumber>111111112222222233333333</AccountNumber> </Account> </Originator>
       <Beneficiary> <Name>Gipsz Jakab</Name> <Account> <AccountNumber>333333334444444455555555</AccountNumber> </Account> </Beneficiary>
       <Amount Currency = ""HUF"" >1000.00</Amount>
       <RequestedExecutionDate>2016-12-02</RequestedExecutionDate>
       <RemittanceInfo> <Text>Utólagos elszámolásra</Text> </RemittanceInfo>
      </Transaction>
";
    public static string TransactionXElem2 = @"
      <Transaction id=""2"" is_selected=""true"">
       <Originator> <Account> <AccountNumber>111111112222222233333333</AccountNumber> </Account> </Originator>
       <Beneficiary> <Name>Gipsz Jakab</Name> <Account> <AccountNumber>333333334444444455555555</AccountNumber> </Account> </Beneficiary>
       <Amount Currency = ""HUF"" >1000.00</Amount>
       <RequestedExecutionDate>2016-12-02</RequestedExecutionDate>
       <RemittanceInfo> <Text>Utólagos elszámolásra</Text> </RemittanceInfo>
      </Transaction>
";
    public static string TransactionXElem3 = @"
      <Transaction id=""3"" is_selected=""true"">
       <Originator> <Account> <AccountNumber>111111112222222233333333</AccountNumber> </Account> </Originator>
       <Beneficiary> <Name>Gipsz Jakab</Name> <Account> <AccountNumber>333333334444444455555555</AccountNumber> </Account> </Beneficiary>
       <Amount Currency = ""HUF"" >1000.00</Amount>
       <RequestedExecutionDate>2016-12-02</RequestedExecutionDate>
       <RemittanceInfo> <Text>Utólagos elszámolásra</Text> </RemittanceInfo>
      </Transaction>
";
    public static string HUFTransactionXml = @"
<HUFTransactions>
" + TransactionXElem1 + TransactionXElem2 + @"
</HUFTransactions>
";
  }
}
