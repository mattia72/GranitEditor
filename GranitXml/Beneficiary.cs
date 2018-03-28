/* 
 Licensed under the Apache License, Version 2.0
 Xml2CSharp.cs
 http://www.apache.org/licenses/LICENSE-2.0
*/

using System;
using System.Xml.Serialization;

namespace GranitXml
{
  [XmlRoot(ElementName = "Beneficiary")]
  public class Beneficiary : IComparable<Beneficiary>, ICloneable
  {
    [XmlElement(ElementName = "Name")]
    public string Name { get; set; }
    [XmlElement(ElementName = "Account")]
    public Account Account { get; set; }

    public Beneficiary()
    {
      Account = new Account();
      Name = "";
    }

    public int CompareTo(Beneficiary other)
    {
      if (0 != Account.CompareTo(other.Account))
        return Account.CompareTo(other.Account);
      if (0 != Name.CompareTo(other.Name))
        return Name.CompareTo(other.Name);
      return 0;
    }

    public object Clone()
    {
      return new Beneficiary { Account = (Account)Account.Clone(), Name = Name };
    }
  }
}