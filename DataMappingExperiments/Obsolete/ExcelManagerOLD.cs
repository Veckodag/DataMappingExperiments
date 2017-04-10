using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using Microsoft.Office.Interop.Excel;

namespace DataMappingExperiments.Obsolete
{
  [Obsolete("Use the newer excelmanager")]
  public class ExcelManagerOLD
  {
    #region Excel Interop

    private readonly Application _excelApp;

    public ExcelManagerOLD()
    {
      _excelApp = new Application();
    }

    public void ExcelConversion(string fileName)
    {
      OpenExcelSpreadsheet(fileName);
    }

    public void OpenExcelSpreadsheet(string fileName)
    {
      try
      {
        var workbook = _excelApp.Workbooks.Open(fileName);
        ExcelScanInternal(workbook);
        workbook.Close();
        Marshal.ReleaseComObject(workbook);
        Marshal.ReleaseComObject(_excelApp);
      }
      catch (Exception)
      {
        Console.WriteLine(" Oh no!");
        Marshal.ReleaseComObject(_excelApp);
        throw;
      }
    }

    private void ExcelScanInternal(Workbook workbook)
    {
      try
      {
        //Only working with the first sheet
        //Excel is not 0 based
        var sheet = (Worksheet)workbook.Sheets[1];
        var excelRange = sheet.UsedRange;

        var rowCount = excelRange.Rows.Count;
        var colCount = excelRange.Columns.Count;
        var cellValue = "";

        // Something like this to retrive cell values
        for (int i = 1; i < rowCount; i++)
        {
          for (int j = 1; j < colCount; j++)
          {
            if (excelRange.Cells[i, j] != null)
            {
              cellValue = (sheet.Cells[i, j] as Range).Value;
              var what = cellValue.GetType();
              Console.Write(what);
            }

            // Excel header (First row in excel)
            //if (i == 1)
            //{

            //}

            //Checking values
            if (cellValue == "Väderskydd")
            {
              Console.Write(cellValue);
            }
          }
        }
      }
      catch (Exception)
      {
        Console.Write(" Failure in the Excel Scan");
        Marshal.ReleaseComObject(_excelApp);
        throw;
      }
    }

    #endregion

    #region XMLSerialization

    private void XmlSerialization()
    {
      //Example on how to seralize XML
      //All classes have to inherit from a base class that is added to the list
      List<object> list = new List<object> { new mySerializableClass() };
      XmlSerializer mySerializer = new XmlSerializer(typeof(object), new[] { typeof(mySerializableClass) });

      using (FileStream fileStream = new FileStream("myFile.xml", FileMode.Create))
      {
        mySerializer.Serialize(fileStream, list);
      }
    }

    internal class mySerializableClass
    {

    }
    //Test for reading in XML
    public T ConvertXml<T>(string xml)
    {
      var serializer = new XmlSerializer(typeof(T));
      return (T)serializer.Deserialize(new StringReader(xml));
    }

    #endregion
  }
}
