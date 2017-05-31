using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.DataMapping;
using DataMappingExperiments.Helpers;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace DataMappingExperiments
{
  public class ExcelManager
  {
    private Mapper _mapper;
    private BIS_GrundObjekt _BisObjekt;
    private List<BIS_GrundObjekt> _BisList;
    public void GetXML(string fileName)
    {
      //TODO: Get the mappingtype from a config file
      _mapper = GetMappingType(MapperType.Plattform);
      _BisList = new List<BIS_GrundObjekt>();

      Console.WriteLine("Importing excel file...");
      ReadExcelFile(fileName);
      CreateObjects();

    }
    /// <summary>
    /// The entry method for XML mapping and structuring
    /// </summary>
    void CreateObjects()
    {
      var container = _mapper.ObjectStructure(_BisList);
      //Make sure the container is loaded
      if (container != null)
        _mapper.Serialization(container);
    }

    //Instantiates a new object of right type
    private BIS_GrundObjekt GetBisObjectType(MapperType mapperType)
    {
      switch (mapperType)
      {
        case MapperType.Plattform:
          return new BIS_Plattform();
        case MapperType.Räl:
          return new BIS_Räl();
        default:
          throw new ArgumentOutOfRangeException(nameof(mapperType), mapperType, null);
      }
    }
    //Sets and instatiates the mappingtype property on the Mapper
    private Mapper GetMappingType(MapperType mapperType)
    {
      switch (mapperType)
      {
        case MapperType.Plattform:
          return new PlattformMapper();
        case MapperType.Räl:
          return new RälMapper();
        default:
          throw new ArgumentOutOfRangeException(nameof(mapperType), mapperType, null);
      }
    }

    #region ExcelInput

    private void ReadExcelFile(string fileName)
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
            return;
          }
          //Adds the columns
          foreach (Cell cell in rowCollection.ElementAt(0))
          {
            //Value of the BIS attribute
            var bisAttribute = GetValueOfCell(spreadsheetDocument, cell);
            dataTable.Columns.Add(bisAttribute);
          }
          //Adds the rows into the dataTable
          foreach (Row row in rowCollection)
          {
            if (row.RowIndex == 1)
              continue;

            DataRow tempRow = dataTable.NewRow();
            int colIndex = 0;

            //A object to use for mapping values
            _BisObjekt = GetBisObjectType(_mapper.MapperType);

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
              var attribute = GetValueOfCell(spreadsheetDocument, cell);
              _BisObjekt = _mapper.MapXmlValue(cellColumnIndex, attribute, _BisObjekt);
              tempRow[colIndex] = attribute;

              colIndex++;
            }
            //The row updates after each cell value
            _BisList.Add(_BisObjekt);
            dataTable.Rows.Add(tempRow);
          }
        }
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
  }
}
