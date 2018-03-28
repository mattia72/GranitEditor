/* 
 Licensed under the Apache License, Version 2.0
 Xml2CSharp.cs
 http://www.apache.org/licenses/LICENSE-2.0
*/

using System;
using System.Xml.Serialization;

namespace GranitXml
{
  [XmlRoot(ElementName = "Account")]
  public class Account : IComparable<Account>, ICloneable
  {
    [XmlElement(ElementName = "AccountNumber")]
    public string AccountNumber { get; set; }
    public Account()
    {
      AccountNumber = "000000000000000000000000";
    }

    public int CompareTo(Account other)
    {
      return AccountNumber.CompareTo(other.AccountNumber);
    }

    public object Clone()
    {
      return new Account { AccountNumber = AccountNumber };
    }
  }
}
