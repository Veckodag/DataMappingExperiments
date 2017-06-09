using System;
using System.Collections.Generic;
using System.Configuration;
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
    public abstract MapperType MapperType { get; set; }
    public abstract string MapXmlAttribute(int index, string attributeValue);
    public abstract BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt bisObject);
    public abstract Container ObjectStructure(List<BIS_GrundObjekt> bisList);

    /// <summary>
    /// Writes out the XML file
    /// </summary>
    /// <param name="container"></param>
    public void Serialization(Container container)
    {
      Console.WriteLine("Generating XML...");
      XmlSerializer serializer = new XmlSerializer(typeof(Container));
      TextWriter tw = new StreamWriter(Program.XmlOutputFile);
      serializer.Serialize(tw, container);
      tw.Close();
      ValidateXML();
    }
    /// <summary>
    /// Validates the XML file against a matching XSD. Reads errors if there are any.
    /// </summary>
    private void ValidateXML()
    {
      var textReader = new StreamReader(Program.XmlOutputFile);
      var xmlDocument = new XmlDocument { Schemas = new XmlSchemaSet() };
      xmlDocument.Schemas.Add(null, new XmlTextReader(StringManager.GetFilePathSetting(Program.XsdFile)));

      xmlDocument.Load(textReader);
      List<string> errors = new List<string>();
      xmlDocument.Validate((sender, EventArgs) => errors.Add(EventArgs.Message));

      ErrorMessage(errors);
      textReader.Close();
    }

    /// <summary>
    /// Console Magic for printing out validation errors.
    /// </summary>
    /// <param name="errors"></param>
    private void ErrorMessage(List<string> errors)
    {
      if (errors.Any())
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Validation Errors Found");
        foreach (var error in errors)
        {
          Console.WriteLine(error);
          Console.WriteLine();
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Please resolve the validation error(s) and try again");
      }
      if (!errors.Any())
      {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("No Valaidation Errors Found");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("XML file complete!");
      }
    }

    public List<UnitInstances> CreateSoftTypeUnitsInstances()
    {
      var unitlist = new List<UnitInstances>();
      //The actual values could come from a config file.
      string[] unitNameString = { "mm", "Procent", "Grader", "st" };

      foreach (var unitName in unitNameString)
      {
        var instance = new UnitEntrydefaultIn
        {
          Array = true,
          id = unitName,
          inputSchemaRef = "defaultIn"
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

      //TODO: Map against proper properties
      string[] properties = { "Hello World!", "THIS_IS_A_TEST_PROPERTY" };

      foreach (var propertyName in properties)
      {
        var instance = new PropertyEntrydefaultIn
        {
          Array = true,
          id = propertyName,
          inputSchemaRef = "defaultIn"
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
    /// Returns a collection of Softtypes that are used as reference keys to other softypes
    /// </summary>
    /// <returns></returns>
    internal List<SoftType> CreateKeyReferences()
    {
      //TODO: Creating softypes instances for keyref
      var softtypeList = new List<SoftType>();

      //Setup
      //Dokument
      var dokument = new DocumentReference_Anläggningsprodukt_dokument
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = new DokumentReference
        {
          softType = "Dokument",
          instanceRef = "Dokument"
        }
      };
      var dokumentLista = new List<DocumentReference_Anläggningsprodukt_dokument> { dokument };

      //Projekt
      var projekt = new ProjectReference_Anläggningsprodukt_projekt
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = new ProjektReference
        {
          softType = "Projekt",
          instanceRef = "Projekt"
        }
      };
      var projektLista = new List<ProjectReference_Anläggningsprodukt_projekt> { projekt };

      //Keyrefs
      //Anläggningsprodukt
      var produktInstance = new AnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "Anläggningsprodukt",
        inputSchemaRef = "defaultIn",
        data = new AnläggningsproduktdefaultIn
        {
          id = "Anläggningsprodukt",
          versionId = "Anläggningsprodukt",
          name = "Anläggningsprodukt",
          notering = "Anläggningsprodukt",
          företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
          {
            @class = new FTAnläggningsproduktReference
            {
              softType = "FTAnläggningsprodukt",
              instanceRef = "FTAnläggningsprodukt"
            },
            startSpecified = false,
            endSpecified = false
          },
          datainsamling = new PropertyValueAssignment_Anläggningsprodukt_datainsamling
          {
            startSpecified = false,
            endSpecified = false
          },
          företeelsetillkomst = new PropertyValueAssignment_Anläggningsprodukt_företeelsetillkomst
          {
            value = "Anläggningsprodukt",
            startSpecified = false,
            endSpecified = false
          },
          ursprung = new PropertyValueAssignment_Anläggningsprodukt_ursprung
          {
            value = "Anläggningsprodukt",
            startSpecified = false,
            endSpecified = false
          },
          dokument = dokumentLista.ToArray(),
          projekt = projektLista.ToArray()
        }
      };
      var produktInstanceLista = new List<AnläggningsproduktInstances> { produktInstance };
      var produkt = new SoftType_Anläggningsprodukt
      {
        instances = produktInstanceLista.ToArray(),
        Array = true,
        id = "Anläggningsprodukt"
      };
      softtypeList.Add(produkt);

      //Bulkvara
      var propertyStringValue = new PropertyValueString
      {
        Array = true,
        value = "Bulkvara",
        generalProperty = new PropertyReference
        {
          softType = "PropertyReference",
          instanceRef = "Property"
        }
      };

      var propertyNumericValue = new PropertyValueNumeric
      {
        Array = true,
        value = 10,
        generalProperty = new PropertyReference
        {
          softType = "PropertyReference",
          instanceRef = "Property"
        }
      };

      var dokumentReference = new DocumentReference_Bulkvara_dokument
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = new DokumentReference
        {
          softType = "Dokument",
          instanceRef = "Dokument"
        }
      };

      var projektReference = new ProjectReference_Bulkvara_projekt
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = new ProjektReference
        {
          softType = "Projekt",
          instanceRef = "Projekt"
        }
      };

      var bulkvaraInstance = new BulkvaraEntrydefaultIn
      {
        Array = true,
        id = "Bulkvara",
        inputSchemaRef = "defaultIn",
        data = new BulkvaradefaultIn
        {
          id = "Bulkvara",
          name = "Bulkvara",
          notering = "Bulkvara",
          versionId = "Bulkvara",
          stringSet = new PropertyValueSetAssignment_Bulkvara_stringSet
          {
            startSpecified = false,
            endSpecified = false,
            value = new[] { propertyStringValue }
          },
          numericSet = new PropertyValueSetAssignment_Bulkvara_numericSet
          {
            startSpecified = false,
            endSpecified = false,
            value = new[] { propertyNumericValue }
          },
          anläggningsprodukt = new ProductDesignVersionToIndividual_Bulkvara_anläggningsprodukt
          {
            startSpecified = false,
            endSpecified = false,
            product = new AnläggningsproduktReference
            {
              softType = "Anläggningsprodukt",
              instanceRef = "Anläggningsprodukt"
            }
          },
          dokument = new[] { dokumentReference },
          projekt = new[] { projektReference },
          företeelsetyp = new ClassificationReference_Bulkvara_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTBulkvaraReference
            {
              instanceRef = "FTBulkvara",
              softType = "FTBulkvara"
            }
          },
          datainsamling = new PropertyValueAssignment_Bulkvara_datainsamling
          {
            startSpecified = false,
            endSpecified = false,
            value = "Bulkvara"
          },
          ursprung = new PropertyValueAssignment_Bulkvara_ursprung
          {
            startSpecified = false,
            endSpecified = false,
            value = "Bulkvara"
          },
          företeelsetillkomst = new PropertyValueAssignment_Bulkvara_företeelsetillkomst
          {
            startSpecified = false,
            endSpecified = false,
            value = "Bulkvara"
          },
          konstateratTillstånd = new StateAssignment_Bulkvara_konstateratTillstånd
          {
            startSpecified = false,
            endSpecified = false,
            bedömning = new StateAssessmentReference
            {
              startSpecified = false,
              endSpecified = false,
              tillstånd = new TillståndsstatusReference
              {
                softType = "Tillståndsstatus",
                instanceRef = "Tillståndsstatus"
              }
            }
          },
          planeradIndivid = new ItemVersionReference_Bulkvara_planeradIndivid
          {
            startSpecified = false,
            endSpecified = false,
            value = new PlaneradIndividReference
            {
              softType = "PlaneradIndivid",
              instanceRef = "PlaneradIndivid"
            }
          },
          prognostiseratTillstånd = new StateAssignment_Bulkvara_prognostiseratTillstånd
          {
            endSpecified = false,
            startSpecified = false,
            bedömning = new StateAssessmentReference
            {
              startSpecified = false,
              endSpecified = false,
              tillstånd = new TillståndsstatusReference
              {
                softType = "Tillståndsstatus",
                instanceRef = "Tillståndsstatus"
              }
            }
          }
        }
      };
      var bulkvaraInstanceLista = new List<BulkvaraInstances> { bulkvaraInstance };
      var bulkvara = new SoftType_Bulkvara
      {
        instances = bulkvaraInstanceLista.ToArray(),
        Array = true,
        id = "Bulkvara"
      };
      softtypeList.Add(bulkvara);
      //Bulkvara end

      var projektInstanceLista = new List<ProjektInstances>();
      var projektSoftype = new SoftType_Projekt
      {
        Array = true,
        id = "Projekt",
        instances = projektInstanceLista.ToArray()
      };

      return softtypeList;
    }

    /// <summary>
    /// Entry point for property key references
    /// </summary>
    /// <returns></returns>
    internal List<SoftType> CreateSupplementarySoftypes()
    {
      var softtypeList = new List<SoftType>();
      var geografiskFTSofttype = new SoftType_FTGeografiskPlaceringsreferens
      {
        Array = true,
        id = "FTGeografiskPlaceringsreferens"
      };
      var FTPlattformar = new List<FTGeografiskPlaceringsreferensInstances>();
      //Vilka softtypes behövs
      var ftPlattformsInstans = new FTGeografiskPlaceringsreferensEntrydefaultIn
      {
        Array = true,
        id = "FTPlattform",
        inputSchemaRef = "defaultIn",
        data = new FTGeografiskPlaceringsreferensdefaultIn
        {
          id = "FTPlattform",
          name = "FTPlattform"
        }
      };
      FTPlattformar.Add(ftPlattformsInstans);
      geografiskFTSofttype.instances = FTPlattformar.ToArray();

      //TODO: Make Real Properties
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
      softtypeList.Add(geografiskFTSofttype);
      softtypeList.Add(softtypeProperty);
      softtypeList.Add(softtypeUnit);

      return softtypeList;
    }
  }
}
