using System;
using System.Web;

namespace DataMappingExperiments.Helpers
{
  public static class StringManager
  {
    public static string FormattingXmlString(string xmlString)
    {
      //Decode the string because of special characters
      xmlString = HttpUtility.HtmlDecode(xmlString);
      xmlString = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + Environment.NewLine + xmlString;
      return xmlString;
    }
  }
}
