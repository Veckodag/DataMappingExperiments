using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DataMappingExperiments.Helpers
{
  public static class ExtensionMethods
  {
    public static string UppercaseFirstLetter(this string value)
    {
      if (value.Length > 0)
      {
        char[] array = value.ToCharArray();
        array[0] = char.ToUpper(array[0]);
        return new string(array);
      }
      return value;
    }

    public static DateTime SkipTheWeekend(this DateTime value)
    {
      while (value.DayOfWeek == DayOfWeek.Saturday || value.DayOfWeek == DayOfWeek.Sunday)
        value = value.AddDays(1);

      return value;
    }

    //Checks if a double is an actual double
    public static bool HasValue(this double value)
    {
      return !double.IsNaN(value) && !double.IsInfinity(value);
    }

    //Removes whitespace at the end of a stringBulder
    public static StringBuilder TrimEnd(this StringBuilder sb)
    {
      if (sb == null || sb.Length == 0) return sb;

      int i = sb.Length - 1;
      for (; i >= 0; i--)
        if (!char.IsWhiteSpace(sb[i]))
          break;

      if (i < sb.Length - 1)
        sb.Length = i + 1;

      return sb;
    }

    public static string GetColumn(this DataRow row, int ordinal)
    {
      return row.Table.Columns[ordinal].ColumnName;
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
      foreach (var item in source)
        action(item);
    }

    public static IEnumerable<t> RandomizeOrder<t>(this IEnumerable<t> target)
    {
      Random r = new Random();
      return target.OrderBy(x => (r.Next()));
    }

    //Checks if a string matches another string
    public static bool ContainsAny(this string theString, char[] characters)
    {
      foreach (char character in characters)
      {
        if (theString.Contains(character.ToString()))
        {
          return true;
        }
      }
      return false;
    }
    public static string ReplaceFirstOccuranceOfString(this string text, string searchString, string replacementString)
    {
      int position = text.IndexOf(searchString, StringComparison.Ordinal);
      if (position < 0) return text;
      return text.Substring(0, position) + replacementString + text.Substring(position + searchString.Length);
    }

    public static string SerializeObject<T>(this T toSerialize)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

      using (StringWriter textWriter = new StringWriter())
      {
        xmlSerializer.Serialize(textWriter, toSerialize);
        return textWriter.ToString();
      }
    }
  }
}
