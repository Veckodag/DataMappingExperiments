﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using DataMappingExperiments.DataMapping;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace DataMappingExperiments
{
  public class ExcelManager
  {
    #region ExcelToXmlString

    private Mapper _mapper;
    public string GetXML(string fileName)
    {
      //Get the name of the Dataset from config
      using (DataSet dataSet = new DataSet("Plattform"))
      {
        _mapper = GetMappingType(MapperType.Plattform);
        dataSet.Tables.Add(ReadExcelFile(fileName));
        dataSet.Namespace = @"http://trafikverket.se/anda/inputschemasföreteelsetyperDx/20170316";
        dataSet.Tables[0].Namespace = "";
        dataSet.Prefix = "anda";
        return dataSet.GetXml();
        //return dataSet.GetXmlSchema();
      }
    }

    public Mapper GetMappingType(MapperType mapperType)
    {
      switch (mapperType)
      {
        case MapperType.Plattform:
          return new PlattformMapper();
        case MapperType.Räl:
          return new RälMapper();
        default:
          Console.WriteLine("Not a mapping type!");
          break;
      }
      return null;
    }
    public enum MapperType
    {
      Plattform,
      Räl
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
          var worksheetPart = (WorksheetPart)workbookPart.GetPartById(relationshipId);

          //Only working with the first sheet
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
            //Value of the BIS attribute
            //TODO: Här ställs XML attributet in
            int cellColumnIndex = GetColumnIndex(GetColumnName(cell.CellReference));

            var bisAttribute = GetValueOfCell(spreadsheetDocument, cell);
            var mappedValue = _mapper.MapXmlAttribute(cellColumnIndex, bisAttribute);
            dataTable.Columns.Add(mappedValue);

          }

          //Adds the rows into the dataTable
          foreach (Row row in rowCollection)
          {
            if (row.RowIndex == 1)
              continue;

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
              //TODO: Känn av index och skicka in det för mappning
              tempRow[colIndex] = GetValueOfCell(spreadsheetDocument, cell);
              colIndex++;
            }
            //The row updates after each cell value
            dataTable.Rows.Add(tempRow);
          }
        }
        //dataTable.Rows.RemoveAt(0);
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
          columnIndex += factor * (columnName[position] - 'A' + 1) - 1;
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

    #region XMLFileCreation

    public string CreateXMLFile(string xmlString)
    {

      string xmlName = "test.xml";
      //Writes a new XML file, unicode to keep swedish characters
      //File.WriteAllText(xmlName, xmlString, Encoding.Unicode);
      return xmlName;
    }

    public void ValidateXML(string xsd, string xmlName)
    {
      XmlSchemaSet schemaSet = new XmlSchemaSet();

      //XSD file with the namespace
      schemaSet.Add("http://trafikverket.se/anda/inputschemasföreteelsetyperDx/20170316", xsd);

      XmlReaderSettings settings = new XmlReaderSettings
      {
        ValidationType = ValidationType.Schema,
        Schemas = schemaSet
      };
      settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
    }

    private void ValidationCallBack(object sender, ValidationEventArgs e)
    {

    }
    #endregion
  }
}
