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
      string detailsFile = ConfigurationManager.AppSettings["DetailsFile"];
      string xsdFilePath = ConfigurationManager.AppSettings["XsdFile"];

      StartExcelManager(detailsFile, xsdFilePath);

      Console.ReadLine();
    }

    static void StartExcelManager(string fileName, string xsdFile)
    {
      if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
      {
        Console.WriteLine("The file is invalid. Select a valid file.");
        return;
      }

      var excelManager = new ExcelManager();
      // Massive string with all the good stuff
      excelManager.GetXML(fileName);

    }
  }
}
