using System;
using System.Configuration;
using System.Web;

namespace DataMappingExperiments.Helpers
{
  public static class StringManager
  {
    public static string XmlOutputFile = ConfigurationManager.AppSettings["XMLOutputFile"];
    public static string XsdFile = ConfigurationManager.AppSettings["XsdFile"];
    public static string FormattingXmlString(string xmlString)
    {
      //Decode the string because of special characters
      xmlString = HttpUtility.HtmlDecode(xmlString);
      xmlString = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + Environment.NewLine + xmlString;
      return xmlString;
    }

    public static string StringDecoding(string xmlString)
    {
      return HttpUtility.HtmlDecode(xmlString);
    }
  }
}
