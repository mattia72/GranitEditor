
using System;
using System.Xml.Serialization;

namespace GranitXml
{
  [XmlRoot(ElementName = "Originator")]
  public class Originator : IComparable<Originator>, ICloneable
  {
    [XmlElement(ElementName = "Account")]
    public Account Account { get; set; }
    public Originator()
    {
      Account = new Account();
    }

    public int CompareTo(Originator other)
    {
      return Account.CompareTo(other.Account);
    }

    public object Clone()
    {
      return new Originator { Account = (Account)Account.Clone() };
    }
  }
}
