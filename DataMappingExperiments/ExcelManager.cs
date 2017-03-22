using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Excel = Microsoft.Office.Interop.Excel;

namespace DataMappingExperiments
{
  public class ExcelManager
  {
    #region ExcelToXmlString

    public string GetXML(string fileName)
    {
      using (DataSet dataSet = new DataSet())
      {
        dataSet.Tables.Add(ReadExcelFile(fileName));
        return dataSet.GetXml();
      }
    }

    private DataTable ReadExcelFile(string fileName)
    {
      DataTable dataTable = new DataTable();

      try
      {
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
          var workbookPart = spreadsheetDocument.WorkbookPart;
          IEnumerable<Sheet> sheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();

          string relationshipId = sheetcollection.First().Id.Value;
          var worksheetPart = (WorksheetPart) workbookPart.GetPartById(relationshipId);

          SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
          IEnumerable<Row> rowCollection = sheetData.Descendants<Row>();

          //When there is no data left to process
          if (!rowCollection.Any())
          {
            return dataTable;
          }

          //Adds the columns
          foreach (Cell cell in rowCollection.ElementAt(0))
          {
            dataTable.Columns.Add(GetValueOfCell(spreadsheetDocument, cell));
          }

          //Adds the rows into the dataTable
          foreach (Row row in rowCollection)
          {
            DataRow tempRow = dataTable.NewRow();
            int colIndex = 0;

            //Every cell in selected row
            foreach (Cell cell in row.Descendants<Cell>())
            {
              int cellColumnIndex = GetColumnIndex(GetColumnName(cell.CellReference));

              while (colIndex < cellColumnIndex)
              {
                //Empties the cell at the right index
                tempRow[colIndex] = string.Empty;
                colIndex++;
              }
              //Then sets the cell value at the right index
              tempRow[colIndex] = GetValueOfCell(spreadsheetDocument, cell);
              colIndex++;
            }
            //The row updates after each cell value
            dataTable.Rows.Add(tempRow);
          }
        }
        dataTable.Rows.RemoveAt(0);
        return dataTable;
      }
      catch (IOException exception)
      {
        throw new IOException(exception.Message);
      }
    }

    //Get index of column from given column name
    private int GetColumnIndex(string columnName)
    {
      int columnIndex = 0, factor = 1;

      //From right to left
      for (int position = columnName.Length - 1; position >= 0; position--)
      {
        if (char.IsLetter(columnName[position]))
        {
          columnIndex += factor*(columnName[position] - 'A' + 1) - 1;
          factor *= 26;
        }
      }
      return columnIndex;
    }

    private string GetColumnName(string cellReference)
    {
      //Makes sure that the Excel column name is valid, ex A1 or AA
      Regex regex = new Regex("[A-Za-z]+");
      Match match = regex.Match(cellReference);
      return match.Value;
    }

    private string GetValueOfCell(SpreadsheetDocument spreadsheetDocument, Cell cell)
    {
      SharedStringTablePart sharedString = spreadsheetDocument.WorkbookPart.SharedStringTablePart;
      if (cell.CellValue == null)
      {
        return string.Empty;
      }

      //Makes sure we return the value (innerText) in the right format
      //as sharedstring or something else
      string cellValue = cell.CellValue.InnerText;

      if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
      {
        var cellText = sharedString.SharedStringTable.ChildElements[int.Parse(cellValue)].InnerText;
        return cellText;
      }
      return cellValue;
    }

    #endregion

    public void CreateXMLFile(string xmlString)
    {
      //XmlDocument xmlDocument = new XmlDocument();
      //xmlDocument.LoadXml(xmlString);
      //xmlDocument.Save("test.xml");

      //File.WriteAllText("test.xml", xmlString, Encoding.ASCII);

      //XmlTextWriter textWriter = new XmlTextWriter(xmlString, Encoding.UTF8);

    }
  }
}
