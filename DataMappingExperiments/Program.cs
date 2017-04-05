using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using XmlSchema = System.Xml.Schema.XmlSchema;

namespace DataMappingExperiments
{
  class Program
  {
    static void Main(string[] args)
    {
      string excelFilePath =
        @"c:\users\fresan\documents\visual studio 2015\Projects\DataMappingExperiments\DataMappingExperiments\Testdata\Fln-Blg Plattformar Enkel.xlsx";
      string xsdFilePath = @"C:\Users\fresan\documents\visual studio 2015\Projects\DataMappingExperiments\DataMappingExperiments\Testdata\ANDAImport.xml";
      //TODO: 1. Take a excel file input
      StartExcelManager(excelFilePath, xsdFilePath);
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

    static void StartExcelManager(string fileName, string xsdFile)
    {
      if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
      {
        Console.WriteLine("The file is invalid. Select a valid file.");  
        return;
      }
      try
      {
        #region ExcelInput

        var excelManager = new ExcelManager();
        // Massive string with all the good stuff
        string xmlString = excelManager.GetXML(fileName);
        //Decode the string because of special characters
        xmlString = HttpUtility.HtmlDecode(xmlString);
        xmlString = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + Environment.NewLine + xmlString;

        if (string.IsNullOrEmpty(xmlString))
        {
          Console.WriteLine("The content of the Excel file is empty!");
        }
        Console.WriteLine(xmlString);
        #endregion
        //Create the XML
        string xmlName = excelManager.CreateXMLFile(xmlString);

        //excelManager.ValidateXML(xsdFile, xmlName);
      }
      catch (Exception exception)
      {
        Console.WriteLine("Something went wrong: " + exception.Message);
      }
    }
  }
}
