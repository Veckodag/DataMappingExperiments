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
    internal string _JsonMapToValue = "value";
    internal string _InputSchemaRef = "defaultIn";
    public abstract MapperType MapperType { get; set; }
    public abstract BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt bisObject);
    public abstract Container ObjectStructure(List<BIS_GrundObjekt> bisList);
    public abstract IEnumerable<BIS_GrundObjekt> SquashTheList(List<BIS_GrundObjekt> bisList);

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

      //Could reset back to ignore validation
      //Console.WriteLine("Validating...");
      //var isValid = ValidateXML();
      //if (isValid) //Who cares about validation, dude!
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
      string[] unitNameString = { "mm", "Procent", "Grader", "st" };

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
        //"Hello World!", "THIS_IS_A_TEST_PROPERTY"
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
    /// Returns a collection of Softtypes that are used as reference keys to other softypes
    /// </summary>
    /// <returns></returns>
    internal List<SoftType> CreateKeyReferences()
    {
      //TODO: Creating softypes instances for keyref
      var softtypeList = new List<SoftType>();
      //Setup
      //Generic Properties
      var propertyStringValue = new PropertyValueString
      {
        Array = true,
        generalProperty = new PropertyReference
        {
          softType = "Property",
          instanceRef = "Property"
        }
      };

      var propertyNumericValue = new PropertyValueNumeric
      {
        Array = true,
        generalProperty = new PropertyReference
        {
          softType = "Property",
          instanceRef = "Property"
        }
      };
      var genericDocumentReference = new DokumentReference
      {
        softType = "Dokument",
        instanceRef = "Dokument"
      };

      var genericProjectReference = new ProjektReference
      {
        softType = "Projekt",
        instanceRef = "Projekt"
      };

      var genericAnläggningsproduktReference = new AnläggningsproduktReference
      {
        softType = "Anläggningsprodukt",
        instanceRef = "Anläggningsprodukt"
      };

      var genericAnläggningsUtrymmeReference = new AnläggningsutrymmeReference
      {
        softType = "Anläggningsutrymme",
        instanceRef = "Anläggningsutrymme"
      };

      //Dokument - NOT SOFTTYPE
      var dokument = new DocumentReference_Anläggningsprodukt_dokument
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = genericDocumentReference
      };
      var dokumentLista = new List<DocumentReference_Anläggningsprodukt_dokument> { dokument };

      //Projekt - NOT SOFTTYPE
      var projekt = new ProjectReference_Anläggningsprodukt_projekt
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = genericProjectReference
      };
      var projektLista = new List<ProjectReference_Anläggningsprodukt_projekt> { projekt };
      //KEYREFS

      //Property
      var propertyInstance = new PropertyEntrydefaultIn
      {
        Array = true,
        id = "Property",
        inputSchemaRef = "defaultIn",
        data = new PropertydefaultIn
        {
          id = "Property",
          name = "Property"
        }
      };
      var propertyInstances = new List<PropertyInstances> { propertyInstance };
      var propertySoftType = new SoftType_Property
      {
        Array = true,
        id = "Property",
        instances = propertyInstances.ToArray()
      };
      softtypeList.Add(propertySoftType);
      //Property END

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

      var dokumentReference = new DocumentReference_Bulkvara_dokument
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = genericDocumentReference
      };

      var projektReference = new ProjectReference_Bulkvara_projekt
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = genericProjectReference
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
          anläggningsprodukt = new ProductDesignVersionToIndividual_Bulkvara_anläggningsprodukt
          {
            startSpecified = false,
            endSpecified = false,
            product = genericAnläggningsproduktReference
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
            endSpecified = false
          },
          ursprung = new PropertyValueAssignment_Bulkvara_ursprung
          {
            startSpecified = false,
            endSpecified = false
          },
          företeelsetillkomst = new PropertyValueAssignment_Bulkvara_företeelsetillkomst
          {
            startSpecified = false,
            endSpecified = false
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

      //Projekt
      var projektInstance = new ProjektEntrydefaultIn
      {
        Array = true,
        id = "Projekt",
        inputSchemaRef = "defaultIn",
        data = new ProjektdefaultIn
        {
          id = "Projekt",
          name = "Projekt",
          notering = "Projekt"
        }
      };

      var projektInstanceLista = new List<ProjektInstances> { projektInstance };
      var projektSoftype = new SoftType_Projekt
      {
        Array = true,
        id = "Projekt",
        instances = projektInstanceLista.ToArray()
      };
      softtypeList.Add(projektSoftype);
      //Projekt end

      //Dokument
      var projektDokumentReference = new ProjectReference_Dokument_projekt
      {
        Array = true,
        endSpecified = false,
        startSpecified = false,
        value = genericProjectReference
      };

      var dokumentDokumentReference = new DocumentReference_Dokument_dokument
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = genericDocumentReference
      };

      var dokumentExternReference = new DocumentReference_Dokument_externReferens
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = new ExternReferensReference
        {
          instanceRef = "ExternReferens",
          softType = "ExternReferens"
        }
      };

      var dokumentInstace = new DokumentEntrydefaultIn
      {
        Array = true,
        id = "Dokument",
        inputSchemaRef = "defaultIn",
        data = new DokumentdefaultIn
        {
          id = "Dokument",
          name = "Dokument",
          notering = "Dokument",
          versionId = "Dokument",
          stringSet = new PropertyValueSetAssignment_Dokument_stringSet
          {
            startSpecified = false,
            endSpecified = false,
            value = new[] { propertyStringValue }
          },
          numericSet = new PropertyValueSetAssignment_Dokument_numericSet(),
          företeelsetyp = new ClassificationReference_Dokument_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTDokumentReference
            {
              softType = "FTDokument",
              instanceRef = "FTDokument"
            }
          },
          datainsamling = new PropertyValueAssignment_Dokument_datainsamling
          {
            startSpecified = false,
            endSpecified = false
          },
          ursprung = new PropertyValueAssignment_Dokument_ursprung
          {
            startSpecified = false,
            endSpecified = false
          },
          företeelsetillkomst = new PropertyValueAssignment_Dokument_företeelsetillkomst
          {
            startSpecified = false,
            endSpecified = false
          },
          projekt = new[] { projektDokumentReference },
          dokument = new[] { dokumentDokumentReference },
          externReferens = new[] { dokumentExternReference }
        }
      };

      var dokumentInstanceLista = new List<DokumentInstances> { dokumentInstace };
      var dokumentSoftType = new SoftType_Dokument
      {
        Array = true,
        id = "Dokument",
        instances = dokumentInstanceLista.ToArray()
      };
      softtypeList.Add(dokumentSoftType);
      //Dokument end

      //Styckevara
      var styckevaraDokumentReference = new DocumentReference_Styckevara_dokument
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = genericDocumentReference
      };

      var styckevaraProjektReference = new ProjectReference_Styckevara_projekt
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = genericProjectReference
      };

      var styckevaraInstance = new StyckevaraEntrydefaultIn
      {
        Array = true,
        id = "Styckevara",
        inputSchemaRef = "defaultIn",
        data = new StyckevaradefaultIn
        {
          id = "Styckevara",
          name = "Styckevara",
          notering = "Styckevara",
          versionId = "Styckevara",
          anläggningsprodukt = new ProductDesignVersionToIndividual_Styckevara_anläggningsprodukt
          {
            startSpecified = false,
            endSpecified = false,
            product = genericAnläggningsproduktReference
          },
          datainsamling = new PropertyValueAssignment_Styckevara_datainsamling
          {
            startSpecified = false,
            endSpecified = false
          },
          företeelsetillkomst = new PropertyValueAssignment_Styckevara_företeelsetillkomst
          {
            startSpecified = false,
            endSpecified = false
          },
          företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTStyckevaraReference
            {
              softType = "FTStyckevara",
              instanceRef = "FTStyckevara"
            }
          },
          planeradIndivid = new ItemVersionReference_Styckevara_planeradIndivid
          {
            startSpecified = false,
            endSpecified = false,

          },
          ursprung = new PropertyValueAssignment_Styckevara_ursprung
          {
            startSpecified = false,
            endSpecified = false
          },
          dokument = new[] { styckevaraDokumentReference },
          projekt = new[] { styckevaraProjektReference }
        }
      };

      var styckevaraInstances = new List<StyckevaraInstances> { styckevaraInstance };
      var styckevaraSoftType = new SoftType_Styckevara
      {
        Array = true,
        id = "Styckevara",
        instances = styckevaraInstances.ToArray()
      };
      softtypeList.Add(styckevaraSoftType);
      //Styckevara END

      //FTFunktionellAnläggning 

      var FTFunktionellAnläggningInstance = new FTFunktionellAnläggningEntrydefaultIn
      {
        Array = true,
        id = "FTFunktionellAnläggning",
        inputSchemaRef = "defaultIn",
        data = new FTFunktionellAnläggningdefaultIn
        {
          id = "FTFunktionellAnläggning",
          name = "FTFunktionellAnläggning"
        }
      };

      var FTFunktionellAnläggningInstances = new List<FTFunktionellAnläggningInstances> { FTFunktionellAnläggningInstance };
      var FTFunktionellAnläggningSoftType = new SoftType_FTFunktionellAnläggning
      {
        Array = true,
        id = "FTFunktionellAnläggning",
        instances = FTFunktionellAnläggningInstances.ToArray()
      };
      softtypeList.Add(FTFunktionellAnläggningSoftType);

      //FTFunktionellAnläggning END

      //Anläggningsspecifikation

      var anläggningsspecifikationDokumentReference = new DocumentReference_Anläggningsspecifikation_dokument
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = genericDocumentReference
      };

      var anläggningsspecifikationProjectReference = new ProjectReference_Anläggningsspecifikation_projekt
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = genericProjectReference
      };

      var anläggningsspecifikationAnläggningsprodukt = new BreakdownElementRealization_Anläggningsspecifikation_anläggningsprodukt
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = genericAnläggningsproduktReference
      };

      var anläggningsspecifikationAnläggningsutrymme = new BreakdownElementRealization_Anläggningsspecifikation_anläggningsutrymme
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = genericAnläggningsUtrymmeReference
      };

      var anläggningsspecifikationInstance = new AnläggningsspecifikationEntrydefaultIn
      {
        Array = true,
        id = "Anläggningsspecifikation",
        inputSchemaRef = "defaultIn",
        data = new AnläggningsspecifikationdefaultIn
        {
          id = "Anläggningsspecifikation",
          name = "Anläggningsspecifikation",
          notering = "Anläggningsspecifikation",
          versionId = "Anläggningsspecifikation",
          datainsamling = new PropertyValueAssignment_Anläggningsspecifikation_datainsamling
          {
            startSpecified = false,
            endSpecified = false
          },
          företeelsetillkomst = new PropertyValueAssignment_Anläggningsspecifikation_företeelsetillkomst
          {
            startSpecified = false,
            endSpecified = false
          },
          företeelsetyp = new ClassificationReference_Anläggningsspecifikation_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTAnläggningsspecifikationReference
            {
              softType = "FTAnläggningsspecifikation",
              instanceRef = "FTAnläggningsspecifikation"
            }
          },
          ursprung = new PropertyValueAssignment_Anläggningsspecifikation_ursprung
          {
            startSpecified = false,
            endSpecified = false
          },
          dokument = new[] { anläggningsspecifikationDokumentReference },
          projekt = new[] { anläggningsspecifikationProjectReference },
          anläggningsprodukt = new[] { anläggningsspecifikationAnläggningsprodukt },
          anläggningsutrymme = new[] { anläggningsspecifikationAnläggningsutrymme }
        }
      };

      var anläggningsspecifikationInstances = new List<AnläggningsspecifikationInstances> { anläggningsspecifikationInstance };
      var anläggningsspecifikationSoftType = new SoftType_Anläggningsspecifikation
      {
        Array = true,
        id = "Anläggningsspecifikation",
        instances = anläggningsspecifikationInstances.ToArray()
      };
      softtypeList.Add(anläggningsspecifikationSoftType);
      //Anläggningsspecifikation END

      //KonstateradTillståndsIndivid

      var konstateradTillståndsIndividDocumentReference = new DocumentReference_KonstateradTillståndsindivid_dokument
      {
        startSpecified = false,
        endSpecified = false,
        value = genericDocumentReference
      };

      var konstateradTillståndsIndividProjectReference = new ProjectReference_KonstateradTillståndsindivid_projekt
      {
        startSpecified = false,
        endSpecified = false,
        value = genericProjectReference
      };

      var konstateradTillståndsIndividInstance = new KonstateradTillståndsindividEntrydefaultIn
      {
        Array = true,
        id = "KonstateradTillståndsindivid",
        inputSchemaRef = "defaultIn",
        data = new KonstateradTillståndsindividdefaultIn
        {
          id = "KonstateradTillståndsindivid",
          name = "KonstateradTillståndsindivid",
          notering = "KonstateradTillståndsindivid",
          versionId = "KonstateradTillståndsindivid",
          datainsamling = new PropertyValueAssignment_KonstateradTillståndsindivid_datainsamling
          {
            startSpecified = false,
            endSpecified = false
          },
          företeelsetillkomst = new PropertyValueAssignment_KonstateradTillståndsindivid_företeelsetillkomst
          {
            startSpecified = false,
            endSpecified = false
          },
          företeelsetyp = new ClassificationReference_KonstateradTillståndsindivid_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTKonstateradTillståndsindividReference
            {
              softType = "FTKonstateradTillståndsindivid",
              instanceRef = "FTKonstateradTillståndsindivid"
            }
          },
          ursprung = new PropertyValueAssignment_KonstateradTillståndsindivid_ursprung
          {
            startSpecified = false,
            endSpecified = false
          },
          stringSet = new PropertyValueSetAssignment_KonstateradTillståndsindivid_stringSet
          {
            startSpecified = false,
            endSpecified = false,
            value = new[] { propertyStringValue }
          },
          numericSet = new PropertyValueSetAssignment_KonstateradTillståndsindivid_numericSet(),
          dokument = new[] { konstateradTillståndsIndividDocumentReference },
          projekt = new[] { konstateradTillståndsIndividProjectReference }
        }
      };

      var konstateradTillståndsIndividInstances = new List<KonstateradTillståndsindividInstances> { konstateradTillståndsIndividInstance };
      var konstateradTillståndsIndividSoftType = new SoftType_KonstateradTillståndsindivid
      {
        Array = true,
        id = "KonstateradTillståndsindivid",
        instances = konstateradTillståndsIndividInstances.ToArray()
      };
      softtypeList.Add(konstateradTillståndsIndividSoftType);
      //KonstateradTillståndsIndivid END

      //PlaneradIndivid
      var planeradIndividDokumentReference = new DocumentReference_PlaneradIndivid_dokument
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = genericDocumentReference
      };

      var planeradIndividProjectReference = new ProjectReference_PlaneradIndivid_projekt
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = genericProjectReference
      };

      var planeradIndividInstance = new PlaneradIndividEntrydefaultIn
      {
        Array = true,
        id = "PlaneradIndivid",
        inputSchemaRef = "defaultIn",
        data = new PlaneradIndividdefaultIn
        {
          id = "PlaneradIndivid",
          name = "PlaneradIndivid",
          notering = "PlaneradIndivid",
          versionId = "PlaneradIndivid",
          anläggningsprodukt = new ProductDesignVersionToIndividual_PlaneradIndivid_anläggningsprodukt
          {
            startSpecified = false,
            endSpecified = false,
            product = genericAnläggningsproduktReference
          },
          datainsamling = new PropertyValueAssignment_PlaneradIndivid_datainsamling
          {
            startSpecified = false,
            endSpecified = false
          },
          företeelsetillkomst = new PropertyValueAssignment_PlaneradIndivid_företeelsetillkomst
          {
            startSpecified = false,
            endSpecified = false
          },
          företeelsetyp = new ClassificationReference_PlaneradIndivid_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTPlaneradIndividReference
            {
              softType = "FTPlaneradIndivid",
              instanceRef = "FTPlaneradIndivid"
            }
          },
          ursprung = new PropertyValueAssignment_PlaneradIndivid_ursprung
          {
            startSpecified = false,
            endSpecified = false
          },
          stringSet = new PropertyValueSetAssignment_PlaneradIndivid_stringSet
          {
            startSpecified = false,
            endSpecified = false,
            value = new[] { propertyStringValue }
          },
          numericSet = new PropertyValueSetAssignment_PlaneradIndivid_numericSet(),
          dokument = new[] { planeradIndividDokumentReference },
          projekt = new[] { planeradIndividProjectReference }
        }
      };
      var planeradIndividInstances = new List<PlaneradIndividInstances> { planeradIndividInstance };
      var planeradIndivid = new SoftType_PlaneradIndivid
      {
        Array = true,
        id = "PlaneradIndivid",
        instances = planeradIndividInstances.ToArray()
      };
      softtypeList.Add(planeradIndivid);
      //PlaneradIndivid END

      //Anläggningsutrymme
      var anläggningsutrymmeDocumentReference = new DocumentReference_Anläggningsutrymme_dokument
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = genericDocumentReference
      };

      var anläggningsutrymmeProjectReference = new ProjectReference_Anläggningsutrymme_projekt
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = genericProjectReference
      };

      var anläggningsutrymmeInstance = new AnläggningsutrymmeEntrydefaultIn
      {
        Array = true,
        id = "Anläggningsutrymme",
        inputSchemaRef = "defaultIn",
        data = new AnläggningsutrymmedefaultIn
        {
          id = "Anläggningsutrymme",
          name = "Anläggningsutrymme",
          notering = "Anläggningsutrymme",
          versionId = "Anläggningsutrymme",
          datainsamling = new PropertyValueAssignment_Anläggningsutrymme_datainsamling
          {
            startSpecified = false,
            endSpecified = false
          },
          företeelsetillkomst = new PropertyValueAssignment_Anläggningsutrymme_företeelsetillkomst
          {
            startSpecified = false,
            endSpecified = false
          },
          företeelsetyp = new ClassificationReference_Anläggningsutrymme_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTAnläggningsutrymmeReference
            {
              softType = "FTAnläggningsutrymme",
              instanceRef = "FTAnläggningsutrymme"
            }
          },
          ursprung = new PropertyValueAssignment_Anläggningsutrymme_ursprung
          {
            startSpecified = false,
            endSpecified = false
          },
          stringSet = new PropertyValueSetAssignment_Anläggningsutrymme_stringSet
          {
            startSpecified = false,
            endSpecified = false,
            value = new[] { propertyStringValue }
          },
          numericSet = new PropertyValueSetAssignment_Anläggningsutrymme_numericSet(),
          dokument = new[] { anläggningsutrymmeDocumentReference },
          projekt = new[] { anläggningsutrymmeProjectReference }
        }
      };
      var anläggningsutrymmeInstances = new List<AnläggningsutrymmeInstances> { anläggningsutrymmeInstance };
      var anläggningsutrymmeSoftType = new SoftType_Anläggningsutrymme
      {
        Array = true,
        id = "Anläggningsutrymme",
        instances = anläggningsutrymmeInstances.ToArray()
      };
      softtypeList.Add(anläggningsutrymmeSoftType);
      //Anläggningsutrymme END

      //FTGeografiskPlaceringsReferens
      var FTGeografiskplaceringsreferensInstance = new FTGeografiskPlaceringsreferensEntrydefaultIn
      {
        Array = true,
        id = "FTGeografiskPlaceringsreferens",
        inputSchemaRef = "defaultIn",
        data = new FTGeografiskPlaceringsreferensdefaultIn
        {
          id = "FTGeografiskPlaceringsreferens",
          name = "FTGeografiskPlaceringsreferens"
        }
      };
      var FTGeografiskPlaceringsreferensInstances = new List<FTGeografiskPlaceringsreferensInstances> { FTGeografiskplaceringsreferensInstance };
      var FTGeografiskPlaceringsReferensSoftType = new SoftType_FTGeografiskPlaceringsreferens
      {
        Array = true,
        id = "FTGeografiskPlaceringsreferens",
        instances = FTGeografiskPlaceringsreferensInstances.ToArray()
      };
      softtypeList.Add(FTGeografiskPlaceringsReferensSoftType);
      //FTGeografiskPlaceringsReferens END

      //FTAnläggningsProdukt
      var FTAnläggningsProdukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "FTAnläggningsprodukt",
        inputSchemaRef = "defaultIn",
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "FTAnläggningsprodukt",
          name = "FTAnläggningsprodukt"
        }
      };
      var FTAnläggningsproduktInstances = new List<FTAnläggningsproduktInstances> { FTAnläggningsProdukt };
      var FTAnläggningsProduktSoftType = new SoftType_FTAnläggningsprodukt
      {
        Array = true,
        id = "FTAnläggningsprodukt",
        instances = FTAnläggningsproduktInstances.ToArray()
      };
      softtypeList.Add(FTAnläggningsProduktSoftType);
      //FTAnläggningsProdukt END

      //FTStyckevara
      var FTStyckevaraInstance = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "FTStyckevara",
        inputSchemaRef = "defaultIn",
        data = new FTStyckevaradefaultIn
        {
          id = "FTStyckevaraInstances",
          name = "FTStyckevaraInstances"
        }
      };

      var FTStyckevaraInstances = new List<FTStyckevaraInstances> { FTStyckevaraInstance };
      var FTStyckevaraSoftType = new SoftType_FTStyckevara
      {
        Array = true,
        id = "FTStyckevara",
        instances = FTStyckevaraInstances.ToArray()
      };
      softtypeList.Add(FTStyckevaraSoftType);
      //FTStyckevara END

      //FTDokument
      var FTDokumentInstance = new FTDokumentEntrydefaultIn
      {
        Array = true,
        id = "FTDokument",
        inputSchemaRef = "defaultIn",
        data = new FTDokumentdefaultIn
        {
          id = "FTDokument",
          name = "FTDokument"
        }
      };
      var FTDokumentInstances = new List<FTDokumentInstances> { FTDokumentInstance };
      var FTDokumentSoftType = new SoftType_FTDokument
      {
        Array = true,
        id = "FTDokument",
        instances = FTDokumentInstances.ToArray()
      };
      softtypeList.Add(FTDokumentSoftType);
      //FTDokument END

      //FTKonstateradTillståndsindivid
      var FTKonstateradTillståndsindividInstance = new FTKonstateradTillståndsindividEntrydefaultIn
      {
        Array = true,
        id = "FTKonstateradTillståndsindivid",
        inputSchemaRef = "defaultIn",
        data = new FTKonstateradTillståndsindividdefaultIn
        {
          id = "FTKonstateradTillståndsindivid",
          name = "FTKonstateradTillståndsindivid"
        }
      };
      var FTKonstateradTillståndsindividInstances = new List<FTKonstateradTillståndsindividInstances> { FTKonstateradTillståndsindividInstance };
      var FTKonstateradTillståndsindividSoftType = new SoftType_FTKonstateradTillståndsindivid
      {
        Array = true,
        id = "FTKonstateradTillståndsindivid",
        instances = FTKonstateradTillståndsindividInstances.ToArray()
      };
      softtypeList.Add(FTKonstateradTillståndsindividSoftType);
      //FTKonstateradTillståndsindivid END

      //FTBulkvara
      var FTBulkvaraInstance = new FTBulkvaraEntrydefaultIn
      {
        Array = true,
        id = "FTBulkvara",
        inputSchemaRef = "defaultIn",
        data = new FTBulkvaradefaultIn
        {
          id = "FTBulkvara",
          name = "FTBulkvara"
        }
      };
      var FTBulkvaraInstances = new List<FTBulkvaraInstances> { FTBulkvaraInstance };
      var FTBulkvaraSoftType = new SoftType_FTBulkvara
      {
        Array = true,
        id = "FTBulkvara",
        instances = FTBulkvaraInstances.ToArray()
      };
      softtypeList.Add(FTBulkvaraSoftType);
      //FTBulkvara END

      //FTAnläggningsspecifikation
      var FTAnläggningsspecifikationInstance = new FTAnläggningsspecifikationEntrydefaultIn
      {
        Array = true,
        id = "FTAnläggningsspecifikation",
        inputSchemaRef = "defaultIn",
        data = new FTAnläggningsspecifikationdefaultIn
        {
          id = "FTAnläggningsspecifikation",
          name = "FTAnläggningsspecifikation"
        }
      };
      var FTAnläggningsspecifikationInstances = new List<FTAnläggningsspecifikationInstances> { FTAnläggningsspecifikationInstance };
      var FTAnläggningsspecifikationSoftType = new SoftType_FTAnläggningsspecifikation
      {
        Array = true,
        id = "FTAnläggningsspecifikation",
        instances = FTAnläggningsspecifikationInstances.ToArray()
      };
      softtypeList.Add(FTAnläggningsspecifikationSoftType);
      //FTAnläggningsspecifikation END

      //ExternReferens
      var externReferensInstance = new ExternReferensEntrydefaultIn
      {
        Array = true,
        id = "ExternReferens",
        inputSchemaRef = "defaultIn",
        data = new ExternReferensdefaultIn
        {
          id = "ExternReferens",
          name = "ExternReferens",
          notering = "ExternReferens",
          file = new ExternalFile_ExternReferens_file
          {
            name = "ExternReferens",
            lokalisering = "ExternReferens"
          }
        }
      };
      var externReferensInstances = new List<ExternReferensInstances> { externReferensInstance };
      var externReferensSoftType = new SoftType_ExternReferens
      {
        Array = true,
        id = "ExternReferens",
        instances = externReferensInstances.ToArray()
      };
      softtypeList.Add(externReferensSoftType);
      //ExternReferens END

      //FTAnläggningsutrymme
      var FTAnläggningsutrymmeInstance = new FTAnläggningsutrymmeEntrydefaultIn
      {
        Array = true,
        id = "FTAnläggningsutrymme",
        inputSchemaRef = "defaultIn",
        data = new FTAnläggningsutrymmedefaultIn
        {
          id = "FTAnläggningsutrymme",
          name = "FTAnläggningsutrymme"
        }
      };
      var FTAnläggningsutrymmeInstances = new List<FTAnläggningsutrymmeInstances> { FTAnläggningsutrymmeInstance };
      var FTAnläggningsUtrymmeSoftType = new SoftType_FTAnläggningsutrymme
      {
        Array = true,
        id = "FTAnläggningsutrymme",
        instances = FTAnläggningsutrymmeInstances.ToArray()
      };
      softtypeList.Add(FTAnläggningsUtrymmeSoftType);
      //FTAnläggningsutrymme END

      //FTPlaneradIndivid

      var FTPlaneradIndividInstance = new FTPlaneradIndividEntrydefaultIn
      {
        Array = true,
        id = "FTPlaneradIndivid",
        inputSchemaRef = "defaultIn",
        data = new FTPlaneradIndividdefaultIn
        {
          id = "FTPlaneradIndivid",
          name = "FTPlaneradIndivid"
        }
      };
      var FTPlaneradIndividInstances = new List<FTPlaneradIndividInstances> { FTPlaneradIndividInstance };
      var FTPlaneradIndivid = new SoftType_FTPlaneradIndivid
      {
        Array = true,
        id = "FTPlaneradIndivid",
        instances = FTPlaneradIndividInstances.ToArray()
      };
      softtypeList.Add(FTPlaneradIndivid);
      //FTPlaneradIndivid END
      var geografiskFTSofttype = new SoftType_FTGeografiskPlaceringsreferens
      {
        Array = true,
        id = "FTGeografiskPlaceringsreferens"
      };
      var FTPlattformar = new List<FTGeografiskPlaceringsreferensInstances>();
      var ftPlattformsInstans = new FTGeografiskPlaceringsreferensEntrydefaultIn
      {
        Array = true,
        id = "FTGeografiskPlaceringsreferens",
        inputSchemaRef = "defaultIn",
        data = new FTGeografiskPlaceringsreferensdefaultIn
        {
          id = "FTGeografiskPlaceringsreferens",
          name = "FTGeografiskPlaceringsreferens"
        }
      };
      FTPlattformar.Add(ftPlattformsInstans);
      geografiskFTSofttype.instances = FTPlattformar.ToArray();
      softtypeList.Add(geografiskFTSofttype);

      return softtypeList;
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
