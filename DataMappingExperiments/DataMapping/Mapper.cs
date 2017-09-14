using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments.DataMapping
{
  [Serializable]
  public abstract class Mapper : IMapper
  {
    internal string _SoftTypeProperty = "Property";
    internal string _SoftTypeUnit = "Unit";
    internal string _JsonMapToValue = "value";
    internal string _InputSchemaRef = "defaultIn";
    internal string _VersionId = "S0000A";
    public abstract MapperType MapperType { get; set; }
    public int ExtraCounter { get; set; } = 1;
    public string FeatureTypeName { get; set; } = "";
    public abstract BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt bisObject);
    public abstract Container ObjectStructure(List<BIS_GrundObjekt> bisList);
    public abstract List<SoftType> CreateFTKeyReferenceSoftTypes();

    public decimal DecimalConverter(string value)
    {
      if (string.IsNullOrEmpty(value))
      {
        return 0;
      }
      return decimal.Parse(value.Replace(".", ","));
    }

    public string NoteringsFix(string value)
    {
      value = string.IsNullOrEmpty(value)
        ? "Ingen Notering"
        : value;

      return value;
    }

    /// <summary>
    /// Writes out the XML file and entry point for JSON conversion
    /// </summary>
    /// <param name="container"></param>
    public void Serialization(Container container)
    {
      Console.WriteLine("Generating XML...");
      XmlSerializer serializer = new XmlSerializer(typeof(Container));
      TextWriter tw = new StreamWriter(Program.xmlOutput);
      serializer.Serialize(tw, container);
      tw.Close();

      XmlMessage();

      //Could reset back to validation
      if (Program.EnableValidation)
      {
        Console.WriteLine("Validating...");
        var isValid = ValidateXML();
        if (isValid)
          XmlToJsonManager.XmlToJson(container);
      }
      else
        XmlToJsonManager.XmlToJson(container);
    }

    private void XmlMessage()
    {
      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine("XML file complete!");
      Console.ForegroundColor = ConsoleColor.Gray;
    }
    /// <summary>
    /// Validates the XML file against a matching XSD. Reads errors if there are any.
    /// </summary>
    private bool ValidateXML()
    {
      var textReader = new StreamReader(Program.xmlOutput);
      var xmlDocument = new XmlDocument { Schemas = new XmlSchemaSet() };
      xmlDocument.Schemas.Add(null, new XmlTextReader(StringManager.GetFilePathSetting(Program.XsdFile)));

      xmlDocument.Load(textReader);
      List<string> errors = new List<string>();
      xmlDocument.Validate((sender, EventArgs) => errors.Add(EventArgs.Message));

      ErrorMessage(errors);
      textReader.Close();

      if (!errors.Any())
        return true;

      return false;
    }

    /// <summary>
    /// Console magic for printing out validation errors.
    /// </summary>
    /// <param name="errors"></param>
    private void ErrorMessage(List<string> errors)
    {
      if (errors.Any())
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Validation Errors Found...");
        if (Program.PringLog)
        {
          using (var writer = new StreamWriter(StringManager.GetFilePathSetting(Program.ErrorLog)))
          {
            writer.WriteLine("Validation errors: ");
            foreach (var error in errors)
            {
              writer.WriteLine(error);
              writer.WriteLine();
            }
          }
          Console.WriteLine("Please resolve the validation error(s). For details check the error log.");
        }
        else
        {
          foreach (var error in errors)
          {
            Console.WriteLine(error);
            Console.WriteLine();
          }
          Console.WriteLine("Please resolve the validation error(s).");
        }
      }
      else if (!errors.Any())
      {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("No Valaidation Errors Found");
      }
    }

    public List<UnitInstances> CreateSoftTypeUnitsInstances()
    {
      var unitlist = new List<UnitInstances>();
      //The actual values could come from a config file.
      string[] unitNameString =
        { "mm", "Procent", "Grader", "st", "m", "kVA", "l", "Ah", "V", "h" };

      foreach (var unitName in unitNameString)
      {
        var instance = new UnitEntrydefaultIn
        {
          Array = true,
          id = unitName,
          inputSchemaRef = _InputSchemaRef
        };
        var unit = new UnitdefaultIn
        {
          acronym = unitName
        };
        instance.data = unit;
        unitlist.Add(instance);
      }
      return unitlist;
    }

    public List<PropertyInstances> CreateSoftTypePropertyInstances()
    {
      var propertyList = new List<PropertyInstances>();

      string[] properties =
      {
        "typ", "verkligHöjd", "höjd", "längd", "bredd", "nominellHöjd", "PlattformBeläggning", "plattformskantMaterial",
        "harSkyddsräcken", "harPlattformsutrustning", "harLedstråk", "harSkyddzon", "harFotsteg", "profiltyp",
        "vikt", "revideradKlassifikation", "tillverkningsprocess", "stålsort", "skarvTyp","SystemID", "åskskyddsnivå",
        "faser", "skyddstransformatorKapacitet", "säkring", "OrtsNätsavtal", "kapacitet", "tankvolym", "längdPassräl",
        "hårdgjord", "partikelmagnetposition", "avståndFrånSkarv", "ID-ICONIS", "TLS-beteckning", "TLS-id", "TLS-terminal",
        "TLS-typ", "TLS-ursprung", "spårspärrNr", "Återgående","drifttid", "kapacitet", "spänning", "effektförbrukning",
        "minKapacitet", "materialKanalisation", "materialLock", "antalvattengångar", "diamRörinfodring", "diameter",
        "diameterFörlängningHögerSida", "diameterFörlängningVänsterSida", "fyllnadshöjdURUK", "längdFörlängningHögerSida",
        "längdFörlängningVänsterSida", "infodring", "materialFörlängningHögerrSida", "materialFörlängningVänsterSida",
        "släntlutningÖverstigande1_1_5", "typFörlängningHögerSida", "typFörlängningVänsterSida", "ursprungligTyp",
        "ursprungligtMaterial", "faunatrumma", "transformatorEffekt",
        //Speciallare
        "Centraltomläggningsbar", "Lokalfrigivningsbarindividuellt", "Gårattspärraiställverk", "DataKatalogsVersion"
      };

      foreach (var propertyName in properties)
      {
        var instance = new PropertyEntrydefaultIn
        {
          Array = true,
          id = propertyName,
          inputSchemaRef = _InputSchemaRef
        };
        var property = new PropertydefaultIn
        {
          id = propertyName,
          name = propertyName
        };
        instance.data = property;
        propertyList.Add(instance);
      }
      return propertyList;
    }

    /// <summary>
    /// Entry point for property key references
    /// </summary>
    /// <returns></returns>
    internal List<SoftType> CreateSupplementarySoftypes()
    {
      var softtypeList = new List<SoftType>();

      var softtypeProperty = new SoftType_Property
      {
        Array = true,
        id = "Property",
        instances = CreateSoftTypePropertyInstances().ToArray()
      };
      var softtypeUnit = new SoftType_Unit
      {
        Array = true,
        id = "Unit",
        instances = CreateSoftTypeUnitsInstances().ToArray()
      };
      //Add them all to the list
      softtypeList.Add(softtypeProperty);
      softtypeList.Add(softtypeUnit);

      return softtypeList;
    }
  }
}
