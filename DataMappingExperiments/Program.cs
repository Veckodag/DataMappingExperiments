using System;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments
{
  public class Program
  {
    internal const string XsdFile = "XsdFileSecondGen";
    internal const string ErrorLog = "ErrorLog";
    public static string sourceFile = "";
    public static string xmlOutput = "";
    public static DataContainer SelectedDataContainer;
    internal static bool PringLog = false;
    internal static bool EnableValidation = false;
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
          return new DataContainer { Input = "PlattformInput", Output = "PlattformOutput", Json = "PlattformJson", MapperType = MapperType.Plattform, Name = "Plattform"};
        case "räl":
          return new DataContainer { Input = "RälInput", Output = "RälOutput", Json = "RälJson", MapperType = MapperType.Räl, Name = "Räl"};
        case "teknikbyggnad":
          return new DataContainer { Input = "TeknikbyggnadInput", Output = "TeknikbyggnadOutput", Json = "TeknikbyggnadJson", MapperType = MapperType.Teknikbyggnad, Name = "Teknikbyggnad"};
        //The cases below are not fully implemented yet
        case "spårspärr":
          return new DataContainer { Input = "SpårspärrInput", Output = "SpårspärrOutput", Json = "SpårspärrJson", MapperType = MapperType.Spårspärr, Name = "Spårspärr"};
        default:
          throw new ArgumentNullException();
      }
    }

    public class DataContainer
    {
      public string Input { get; set; }
      public string Output { get; set; }
      public string Json { get; set; }
      public string Name { get; set; }
      public MapperType MapperType { get; set; }
    }
  }
}
