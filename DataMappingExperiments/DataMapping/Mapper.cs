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
        "ursprungligtMaterial", "faunatrumma",
        //Speciallare
        "Centraltomläggningsbar", "Lokalfrigivningsbarindividuellt", "Gårattspärraiställverk"
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
      var softtypeList = new List<SoftType>();
      //Setup
      //Generic Properties
      var propertyStringValue = new PropertyValueString
      {
        Array = true,
        generalProperty = new PropertyReference
        {
          softType = "Property",
          instanceRef = Program.SelectedDataContainer.Name
        }
      };

      var genericAnläggningsproduktReference = new AnläggningsproduktReference
      {
        softType = "Anläggningsprodukt",
        instanceRef = "AnläggningsproduktEntrydefaultIn"
      };

      var genericAnläggningsUtrymmeReference = new AnläggningsutrymmeReference
      {
        softType = "Anläggningsutrymme",
        instanceRef = "AnläggningsutrymmeEntrydefaultIn"
      };
      //KEYREFS

      //Anläggningsprodukt
      var produktInstance = new AnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "AnläggningsproduktEntrydefaultIn",
        inputSchemaRef = _InputSchemaRef,
        data = new AnläggningsproduktdefaultIn
        {
          id = "Anläggningsprodukt",
          versionId = _VersionId,
          företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
          {
            @class = new FTAnläggningsproduktReference
            {
              softType = "FTAnläggningsprodukt",
              instanceRef = Program.SelectedDataContainer.Name
            },
            startSpecified = false,
            endSpecified = false
          }
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
      var bulkvaraInstance = new BulkvaraEntrydefaultIn
      {
        Array = true,
        id = "BulkvaraEntrydefaultIn",
        inputSchemaRef = _InputSchemaRef,
        data = new BulkvaradefaultIn
        {
          id = "Bulkvara",
          versionId = _VersionId,
          anläggningsprodukt = new ProductDesignVersionToIndividual_Bulkvara_anläggningsprodukt
          {
            startSpecified = false,
            endSpecified = false,
            product = genericAnläggningsproduktReference
          },
          företeelsetyp = new ClassificationReference_Bulkvara_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTBulkvaraReference
            {
              softType = "FTBulkvara",
              instanceRef = Program.SelectedDataContainer.Name
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
              instanceRef = "PlaneradIndividEntrydefaultIn"
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

      //Styckevara
      var styckevaraInstance = new StyckevaraEntrydefaultIn
      {
        Array = true,
        id = "StyckevaraEntrydefaultIn",
        inputSchemaRef = _InputSchemaRef,
        data = new StyckevaradefaultIn
        {
          id = "Styckevara",
          versionId = _VersionId,
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
              instanceRef = Program.SelectedDataContainer.Name
            }
          },
          planeradIndivid = new ItemVersionReference_Styckevara_planeradIndivid
          {
            startSpecified = false,
            endSpecified = false,
            value = new PlaneradIndividReference
            {
              softType = "PlaneradIndivid",
              instanceRef = "PlaneradIndividEntrydefaultIn"
            }
          },
          ursprung = new PropertyValueAssignment_Styckevara_ursprung
          {
            startSpecified = false,
            endSpecified = false
          }
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
      //Anläggningsspecifikation
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
        id = "AnläggningsspecifikationEntrydefaultIn",
        inputSchemaRef = _InputSchemaRef,
        data = new AnläggningsspecifikationdefaultIn
        {
          id = "Anläggningsspecifikation",
          versionId = _VersionId,
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
              instanceRef = Program.SelectedDataContainer.Name
            }
          },
          ursprung = new PropertyValueAssignment_Anläggningsspecifikation_ursprung
          {
            startSpecified = false,
            endSpecified = false
          },
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
      var konstateradTillståndsIndividInstance = new KonstateradTillståndsindividEntrydefaultIn
      {
        Array = true,
        id = "KonstateradTillståndsindividEntrydefaultIn",
        inputSchemaRef = _InputSchemaRef,
        data = new KonstateradTillståndsindividdefaultIn
        {
          id = "KonstateradTillståndsindivid",
          versionId = _VersionId,
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
              instanceRef = Program.SelectedDataContainer.Name
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
          numericSet = new PropertyValueSetAssignment_KonstateradTillståndsindivid_numericSet()
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
      var planeradIndividInstance = new PlaneradIndividEntrydefaultIn
      {
        Array = true,
        id = "PlaneradIndividEntrydefaultIn",
        inputSchemaRef = _InputSchemaRef,
        data = new PlaneradIndividdefaultIn
        {
          id = "PlaneradIndivid",
          versionId = _VersionId,
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
              instanceRef = Program.SelectedDataContainer.Name
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
          numericSet = new PropertyValueSetAssignment_PlaneradIndivid_numericSet()
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

      var anläggningsutrymmeInstance = new AnläggningsutrymmeEntrydefaultIn
      {
        Array = true,
        id = "AnläggningsutrymmeEntrydefaultIn",
        inputSchemaRef = _InputSchemaRef,
        data = new AnläggningsutrymmedefaultIn
        {
          id = "Anläggningsutrymme",
          versionId = _VersionId,
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
              instanceRef = Program.SelectedDataContainer.Name
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
          numericSet = new PropertyValueSetAssignment_Anläggningsutrymme_numericSet()
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
      return softtypeList;
    }

    internal List<SoftType> CreateFeatureTypesSoftTypes()
    {
      var softtypeList = new List<SoftType>();
      //FTFunktionellAnläggning
      var FTFunktionellAnläggningInstance = new FTFunktionellAnläggningEntrydefaultIn
      {
        Array = true,
        id = Program.SelectedDataContainer.Name,
        inputSchemaRef = _InputSchemaRef,
        data = new FTFunktionellAnläggningdefaultIn
        {
          id = Program.SelectedDataContainer.Name + "FTFunktionellAnläggningdefaultIn",
          name = Program.SelectedDataContainer.Name
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

      //FTGeografiskPlaceringsReferens
      var FTGeografiskplaceringsreferensInstance = new FTGeografiskPlaceringsreferensEntrydefaultIn
      {
        Array = true,
        id = Program.SelectedDataContainer.Name,
        inputSchemaRef = _InputSchemaRef,
        data = new FTGeografiskPlaceringsreferensdefaultIn
        {
          id = Program.SelectedDataContainer.Name + "FTGeografiskPlaceringsreferensdefaultIn",
          name = Program.SelectedDataContainer.Name
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
        id = Program.SelectedDataContainer.Name,
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = Program.SelectedDataContainer.Name + "FTAnläggningsproduktdefaultIn",
          name = Program.SelectedDataContainer.Name
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
        id = Program.SelectedDataContainer.Name,
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = Program.SelectedDataContainer.Name + "FTStyckevaradefaultIn",
          name = Program.SelectedDataContainer.Name
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
        id = Program.SelectedDataContainer.Name,
        inputSchemaRef = _InputSchemaRef,
        data = new FTDokumentdefaultIn
        {
          id = Program.SelectedDataContainer.Name + "FTDokumentdefaultIn",
          name = Program.SelectedDataContainer.Name
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
        id = Program.SelectedDataContainer.Name,
        inputSchemaRef = _InputSchemaRef,
        data = new FTKonstateradTillståndsindividdefaultIn
        {
          id = Program.SelectedDataContainer.Name + "FTKonstateradTillståndsindividdefaultIn",
          name = Program.SelectedDataContainer.Name
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
        id = Program.SelectedDataContainer.Name,
        inputSchemaRef = _InputSchemaRef,
        data = new FTBulkvaradefaultIn
        {
          id = Program.SelectedDataContainer.Name + "FTBulkvaradefaultIn",
          name = Program.SelectedDataContainer.Name
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
        id = Program.SelectedDataContainer.Name,
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsspecifikationdefaultIn
        {
          id = Program.SelectedDataContainer.Name + "FTAnläggningsspecifikationdefaultIn",
          name = Program.SelectedDataContainer.Name
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

      //FTAnläggningsutrymme
      var FTAnläggningsutrymmeInstance = new FTAnläggningsutrymmeEntrydefaultIn
      {
        Array = true,
        id = Program.SelectedDataContainer.Name,
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsutrymmedefaultIn
        {
          id = Program.SelectedDataContainer.Name + "FTAnläggningsutrymmedefaultIn",
          name = Program.SelectedDataContainer.Name
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

      //FTMaterialkomposit
      var FTMaterialkompositInstance = new FTMaterialkompositEntrydefaultIn
      {
        Array = true,
        id = Program.SelectedDataContainer.Name,
        inputSchemaRef = _InputSchemaRef,
        data = new FTMaterialkompositdefaultIn
        {
          id = Program.SelectedDataContainer.Name + "FTMaterialkompositdefaultIn",
          name = Program.SelectedDataContainer.Name
        }
      };
      var FTMaterialkompositInstances = new List<FTMaterialkompositInstances> { FTMaterialkompositInstance };
      var FTMaterialkompositSoftType = new SoftType_FTMaterialkomposit
      {
        Array = true,
        id = "FTMaterialkomposit",
        instances = FTMaterialkompositInstances.ToArray()
      };
      softtypeList.Add(FTMaterialkompositSoftType);
      //FTMaterialkomposit END

      //FTPlaneradIndivid
      var FTPlaneradIndividInstance = new FTPlaneradIndividEntrydefaultIn
      {
        Array = true,
        id = Program.SelectedDataContainer.Name,
        inputSchemaRef = _InputSchemaRef,
        data = new FTPlaneradIndividdefaultIn
        {
          id = Program.SelectedDataContainer.Name + "FTPlaneradIndividdefaultIn",
          name = Program.SelectedDataContainer.Name
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
