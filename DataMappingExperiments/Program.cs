using System;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments
{
  class Program
  {
    
    internal const string PlattformInput = "PlattformInput";
    internal const string PlattformOutput = "PlattformOutput";
    internal const string RälOutputFile = "RälOutput";
    internal const string XsdFile = "XsdFile";
    internal const string JsonFile = "JsonOutput";

    public static string sourceFile = "";
    public static string xmlOutput = "";
    static void Main(string[] args)
    {
      //Add a option to GetFilePath to create new if not existing
      sourceFile = StringManager.GetFilePathSetting(PlattformInput);
      xmlOutput = StringManager.GetFilePathSetting(PlattformOutput);
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
