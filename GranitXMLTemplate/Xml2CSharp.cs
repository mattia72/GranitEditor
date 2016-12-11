/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Xml2CSharp
{
    [XmlRoot(ElementName = "Account")]
    public class Account
    {
        [XmlElement(ElementName = "AccountNumber")]
        public string AccountNumber { get; set; }
        public Account()
        {
            AccountNumber = "";
        }
    }

    [XmlRoot(ElementName = "Originator")]
    public class Originator
    {
        [XmlElement(ElementName = "Account")]
        public Account Account { get; set; }
        public Originator()
        {
            Account = new Account();
        }
    }

    [XmlRoot(ElementName = "Beneficiary")]
    public class Beneficiary
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Account")]
        public Account Account { get; set; }

        public Beneficiary()
        {
            Account = new Account();
        }
    }

    [XmlRoot(ElementName = "Amount")]
    public class Amount
    {
        [XmlAttribute(AttributeName = "Currency")]
        public string Currency { get; set; }
        [XmlText]
        public decimal Value { get; set; }

        public Amount()
        {
            Value = 0;
            Currency = "HUF";
        }

        public Amount(decimal amount, string currency = "HUF")
        {
            Value = amount;
            Currency = currency;
        }
    }

    [XmlRoot(ElementName = "RemittanceInfo")]
    public class RemittanceInfo
    {
        [XmlElement(ElementName = "Text")]
        public List<string> Text { get; set; }
    }

    [XmlRoot(ElementName = "Transaction")]
    public class Transaction
    {
        [XmlElement(ElementName = "Originator")]
        public Originator Originator { get; set; }
        [XmlElement(ElementName = "Beneficiary")]
        public Beneficiary Beneficiary { get; set; }
        [XmlElement(ElementName = "Amount")]
        public Amount Amount { get; set; }
        [XmlElement(ElementName = "RequestedExecutionDate")]
        public string RequestedExecutionDate { get; set; }
        [XmlElement(ElementName = "RemittanceInfo")]
        public RemittanceInfo RemittanceInfo { get; set; }
    }

    [XmlRoot(ElementName = "HUFTransactions")]
    public class HUFTransactions
    {
        [XmlElement(ElementName = "Transaction")]
        public List<Transaction> Transaction { get; set; }
        public HUFTransactions()
        {
            Transaction = new List<Transaction>();
        }
    }

}
