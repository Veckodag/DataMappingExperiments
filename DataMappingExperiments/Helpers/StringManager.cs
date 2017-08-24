using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace DataMappingExperiments.Helpers
{
  public static class StringManager
  {
    public static string GetFilePathSetting(string settingKey)
    {
      string filePath = ConfigurationManager.AppSettings[settingKey];
      if (settingKey == "ErrorLog" && Program.PringLog)
      {
        var time = DateTime.Now.ToString("yy-MM-dd hhmmss");
        var errorFilePath = $@"C:\ANDA {time} ErrorLog.txt";
        File.Create(errorFilePath).Dispose();
        filePath = errorFilePath;
      }

      if (!File.Exists(filePath))
      {
        File.Create(filePath).Dispose();
      }
      else if (string.IsNullOrEmpty(filePath))
      {
        Console.WriteLine("The file is invalid. Select a valid file.");
        return "";
      }
      return filePath;
    }
  }
}
