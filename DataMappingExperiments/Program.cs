using System;
using System.IO;
using System.Xml;
using XmlSchema = System.Xml.Schema.XmlSchema;

namespace DataMappingExperiments
{
  class Program
  {
    static void Main(string[] args)
    {
      string excelFile =
        @"c:\users\fresan\documents\visual studio 2015\Projects\DataMappingExperiments\DataMappingExperiments\Testdata\Fln-Blg Plattformar Enkel.xlsx";
      string xsd = @"C:\Users\fresan\documents\visual studio 2015\Projects\DataMappingExperiments\DataMappingExperiments\Testdata\ANDAImport.xsd";
      //TODO: 1. Take a excel file input
      StartExcelManager(excelFile);
      //TODO: 2. Map the data from the excel against a class
      //TODO: 3. Output data into a XML format
      Console.ReadLine();
    }

    //https://code.msdn.microsoft.com/office/How-to-convert-excel-file-7a9bb404
    //https://github.com/ExcelDataReader/ExcelDataReader
    //https://www.codeproject.com/articles/10581/convert-excel-to-xml-file-xml-schema-and-validate
    //https://coderwall.com/p/app3ya/read-excel-file-in-c

    //How to validate XML against a XSD
    //http://stackoverflow.com/questions/10025986/validate-xml-against-xsd-in-a-single-method
    //https://msdn.microsoft.com/en-us/library/system.xml.schema.validationeventargs.severity.aspx

    static void StartExcelManager(string fileName)
    {
      if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
      {
        Console.WriteLine("The file is invalid. Select a valid file.");  
        return;
      }
      try
      {
        var excelManager = new ExcelManager();
        // Massive string with all the good stuff
        string xmlString = excelManager.GetXML(fileName);
        xmlString = xmlString.Replace("&amp;", "&");

        if (string.IsNullOrEmpty(xmlString))
        {
          Console.WriteLine("The content of the Excel file is empty!");
        }

        Console.WriteLine(xmlString);

        excelManager.CreateXMLFile(xmlString);

      }
      catch (Exception exception)
      {
        Console.WriteLine("Something went wrong: " + exception.Message);
      }
    }
  }
}
