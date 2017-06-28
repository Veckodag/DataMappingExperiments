using System;
using System.Configuration;
using System.IO;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments
{
  class Program
  {
    
    internal const string PlattformInput = "PlattformInput";
    internal const string PlattformOutput = "PlattformOutput";
    internal const string RälOutputFile = "RälOutput";
    internal const string XsdFile = "XsdFile";

    public static string xmlOutput = "";
    static void Main(string[] args)
    {
      string sourceFile = StringManager.GetFilePathSetting(PlattformInput);

      StartExcelManager(sourceFile);

      Console.ReadLine();
    }

    static void StartExcelManager(string fileName)
    {
      var excelManager = new ExcelManager();
      //Massive string with all the good stuff
      excelManager.GetXML(fileName);
    }
  }
}
