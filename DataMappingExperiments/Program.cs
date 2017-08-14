using System;
using System.Collections;
using System.Collections.Generic;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments
{
  public class Program
  {
    internal const string XsdFile = "XsdFile";
    internal const string ErrorLog = "ErrorLog";
    public static string sourceFile = "";
    public static string xmlOutput = "";
    public static DataContainer SelectedDataContainer;
    static void Main(string[] args)
    {
      //Set the datatype once here
      SelectedDataContainer = DataContainerType("plattform");
      sourceFile = StringManager.GetFilePathSetting(SelectedDataContainer.Input);
      xmlOutput = StringManager.GetFilePathSetting(SelectedDataContainer.Output);
      StartManager(sourceFile, SelectedDataContainer.MapperType);

      Console.WriteLine("Program Finished");
      Console.ReadLine();
    }

    static void StartManager(string fileName, MapperType mapperType)
    {
      var excelManager = new ExcelManager(mapperType);
      //Massive string with all the good stuff
      excelManager.GetXML(fileName);
    }

    /// <summary>
    /// Returns related type-data for a specific valuetype
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    static DataContainer DataContainerType(string typeName)
    {
      typeName = typeName.ToLower();
      switch (typeName)
      {
        case "plattform":
          return new DataContainer { Input = "PlattformInput", Output = "PlattformOutput", Json = "PlattformJson", MapperType = MapperType.Plattform };
        case "räl":
          return new DataContainer { Input = "RälInput", Output = "RälOutput", Json = "RälJson", MapperType = MapperType.Räl };
          //The cases below are not fully implemented yet
        case "spårspärr":
          return new DataContainer { Input = "SpårspärrInput", Output = "SpårspärrOutput", Json = "SpårspärrJson", MapperType = MapperType.Spårspärr };
        case "teknikbyggnad":
          return new DataContainer { Input = "TeknikbyggnadInput", Output = "TeknikbyggnadOutput", Json = "TeknikbyggnadJson", MapperType = MapperType.Teknikbyggnad };
        default:
          throw new ArgumentNullException("Please set a datatype and try again!");
      }
    }

    public class DataContainer
    {
      public string Input { get; set; }
      public string Output { get; set; }
      public string Json { get; set; }
      public MapperType MapperType { get; set; }
    }
  }
}
