using System;
using System.Configuration;
using System.IO;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments
{
  class Program
  {
    static void Main(string[] args)
    {
      string excelFilePath = ConfigurationManager.AppSettings["ExcelFile"];
      string extraExcelFile = ConfigurationManager.AppSettings["SecondaryFile"];
      string xsdFilePath = ConfigurationManager.AppSettings["XsdFile"];

      StartExcelManager(excelFilePath, xsdFilePath, excelFilePath);

      Console.ReadLine();
    }

    static void StartExcelManager(string fileName, string xsdFile, string optional = "")
    {
      if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
      {
        Console.WriteLine("The file is invalid. Select a valid file.");
        return;
      }

      #region ExcelInput&Output

      var excelManager = new ExcelManager();
      // Massive string with all the good stuff
      excelManager.GetXML(fileName, optional);
      #endregion
    }
  }
}
