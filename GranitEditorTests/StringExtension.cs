using System;
using System.Linq;

namespace ExtensionMethods
{
  public static class StringExtension
  {
    public static bool IgnoreWhiteSpaceEquals(this string s1, string s2)
    {
      return Enumerable.SequenceEqual(
          s1.Where(c => !char.IsWhiteSpace(c)),
          s2.Where(c => !char.IsWhiteSpace(c)));
    }

    public static bool IgnoreCaseAndWhiteSpaceEquals(this string s1, string s2)
    {
      return Enumerable.SequenceEqual(
          s1.Where(c => !char.IsWhiteSpace(c)).Select(char.ToUpperInvariant),
          s2.Where(c => !char.IsWhiteSpace(c)).Select(char.ToUpperInvariant));
    }
  }
}
