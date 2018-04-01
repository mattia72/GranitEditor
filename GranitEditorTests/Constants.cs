namespace GranitXMLEditorTests
{
  public class TestConstants
  {
    public static string TransactionXml = @"  
      <Transaction>
        <Originator> <Account> <AccountNumber>111111112222222200000000</AccountNumber> </Account> </Originator>
        <Beneficiary> <Name>Gipsz Jakab</Name> <Account> <AccountNumber>333333334444444400000000</AccountNumber> </Account> </Beneficiary>
        <Amount Currency = ""HUF"" >100.00</Amount>
        <RequestedExecutionDate>2016-12-01</RequestedExecutionDate>
        <RemittanceInfo> <Text>Közlemény 01</Text> </RemittanceInfo>
      </Transaction>
      ";

    public static string TransactionXElem1 = @"
      <Transaction id=""1"" is_selected=""true"">
        <Originator> <Account> <AccountNumber>111111112222222211111111</AccountNumber> </Account> </Originator>
        <Beneficiary> <Name>Gipsz Jakab</Name> <Account> <AccountNumber>333333334444444411111111</AccountNumber> </Account> </Beneficiary>
        <Amount Currency = ""HUF"" >1001.00</Amount>
        <RequestedExecutionDate>2016-12-02</RequestedExecutionDate>
        <RemittanceInfo> <Text>Közlemény 02</Text> </RemittanceInfo>
      </Transaction>
      ";

    public static string TransactionXElem2 = @"
      <Transaction id=""2"" is_selected=""true"">
        <Originator> <Account> <AccountNumber>111111112222222222222222</AccountNumber> </Account> </Originator>
        <Beneficiary> <Name>Gipsz Jakab</Name> <Account> <AccountNumber>333333334444444422222222</AccountNumber> </Account> </Beneficiary>
        <Amount Currency = ""HUF"" >2002.00</Amount>
        <RequestedExecutionDate>2016-12-03</RequestedExecutionDate>
        <RemittanceInfo> <Text>Közlemény 03</Text> </RemittanceInfo>
      </Transaction>
      ";

    public static string TransactionXElem3 = @"
      <Transaction id=""3"" is_selected=""true"">
        <Originator> <Account> <AccountNumber>111111112222222233333333</AccountNumber> </Account> </Originator>
        <Beneficiary> <Name>Gipsz Jakab</Name> <Account> <AccountNumber>333333334444444433333333</AccountNumber> </Account> </Beneficiary>
        <Amount Currency = ""HUF"" >3003.00</Amount>
        <RequestedExecutionDate>2016-12-04</RequestedExecutionDate>
        <RemittanceInfo> <Text>Közlemény 04</Text> </RemittanceInfo>
      </Transaction>
      ";

    public static string TransactionXElem4 = @"
      <Transaction id=""4"" is_selected=""true"">
        <Originator> <Account> <AccountNumber>111111112222222244444444</AccountNumber> </Account> </Originator>
        <Beneficiary> <Name>Gipsz Jakab</Name> <Account> <AccountNumber>333333334444444444444444</AccountNumber> </Account> </Beneficiary>
        <Amount Currency = ""HUF"" >4004.00</Amount>
        <RequestedExecutionDate>2016-12-05</RequestedExecutionDate>
        <RemittanceInfo> <Text>Közlemény 05</Text> </RemittanceInfo>
      </Transaction>
      ";

    public static string HUFTransactionXml = @"
      <HUFTransactions> " + 
        TransactionXElem2 + 
        TransactionXElem1 + 
        TransactionXElem3 + @"
      </HUFTransactions>
      ";
  }
}
