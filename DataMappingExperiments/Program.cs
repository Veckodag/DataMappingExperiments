using System;
using System.Configuration;
using System.IO;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments
{
  class Program
  {

    internal const string SourceFile = "SourceFile";
    internal const string XmlOutputFile = "XMLOutputFile";
    internal const string XsdFile = "XsdFile";

    static void Main(string[] args)
    {
      string sourceFile = StringManager.GetFilePathSetting(SourceFile);

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
