﻿using System;
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
      string xsdFilePath = ConfigurationManager.AppSettings["XsdFile"];

      StartExcelManager(excelFilePath, xsdFilePath);
      
      Console.ReadLine();
    }

    static void StartExcelManager(string fileName, string xsdFile)
    {
      if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
      {
        Console.WriteLine("The file is invalid. Select a valid file.");  
        return;
      }

        #region ExcelInput

        var excelManager = new ExcelManager();
        // Massive string with all the good stuff
        string xmlString = excelManager.GetXML(fileName);
       
        xmlString = StringManager.FormattingXmlString(xmlString);

        if (string.IsNullOrEmpty(xmlString))
        {
          Console.WriteLine("The content of the Excel file is empty!");
        }
        Console.WriteLine(xmlString);
        #endregion
        //Create the XML
        //string xmlName = excelManager.CreateXMLFile(xmlString);

    }
  }
}
