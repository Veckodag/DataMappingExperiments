using System;
using System.Configuration;
using System.IO;
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

    public static string GetFilePathSetting(string settingKey)
    {
      Program.xmlOutput = settingKey;
      string filePath = ConfigurationManager.AppSettings[settingKey];

      if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
      {
        Console.WriteLine("The file is invalid. Select a valid file.");
        return "";
      }
      return filePath;
    }

    public static string StringDecoding(string xmlString)
    {
      return HttpUtility.HtmlDecode(xmlString);
    }
  }
}
