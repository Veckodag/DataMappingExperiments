using System;
using System.Collections.Generic;
using System.Linq;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments.DataMapping
{
  public class RälMapper : Mapper
  {
    public RälMapper()
    {
      MapperType = MapperType.Räl;
    }
    public sealed override MapperType MapperType { get; set; }
    public override BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt bisObject)
    {
      var myRäl = (BIS_Räl)bisObject;

      switch (index)
      {
        case 0:
          myRäl.ObjektTypNummer = attributeValue;
          break;
        case 1:
          myRäl.ObjektNummer = attributeValue;
          break;
        case 28:
          myRäl.SidaHVB = attributeValue;
          break;
        case 29:
          myRäl.Rälmodell = attributeValue;
          break;
        case 30:
          myRäl.Vikt = attributeValue;
          break;
        case 31:
          myRäl.Längd = attributeValue;
          break;
        case 32:
          myRäl.Skarvtyp = attributeValue;
          break;
        case 33:
          myRäl.Inläggningsår = attributeValue;
          break;
        case 34:
          myRäl.Tillverkngingsår = attributeValue;
          break;
        case 35:
          myRäl.Rev_Klass = attributeValue;
          break;
        case 36:
          myRäl.Tillverkare = attributeValue;
          break;
        case 37:
          myRäl.Stålsort = attributeValue;
          break;
        case 38:
          myRäl.Tillv_Process = attributeValue;
          break;
        case 39:
          myRäl.Notering = attributeValue;
          break;
        case 40:
          myRäl.Senast_Ändrad = attributeValue;
          break;
        case 41:
          myRäl.Senast_Ändrad_Av = attributeValue;
          break;
      }
      return myRäl;
    }

    public override Container ObjectStructure(List<BIS_GrundObjekt> bisList)
    {
      var formattedBisList = bisList;
      Container container = new Container();
      var containerSoftypes = new List<SoftType>();

      var rälAnläggningsSpecifikationer = new List<AnläggningsspecifikationInstances>();
      var rälAnläggningsProdukter = new List<AnläggningsproduktInstances>();
      var rälBulkvaror = new List<BulkvaraInstances>();
      foreach (BIS_Räl bisRäl in formattedBisList)
      {
        var suffix = bisRäl.ObjektTypNummer + bisRäl.ObjektNummer + ExtraCounter;
        //Noterings fix
        if (string.IsNullOrEmpty(bisRäl.Notering))
        {
          bisRäl.Notering = "Ingen notering";
        }
        //Allt nytt
        var rälSpec = new Rälspecifikation
        {
          name = "Rälspecifikation",
          notering = bisRäl.Notering,
          versionId = _VersionId,
          numericSet = new RälspecifikationNumericSet(),
          stringSet = new RälspecifikationStringSet
          {
            skarvTyp = SkapaSkarvTyp(bisRäl, new Rälspecifikation_skarvTyp()),
            typ = new Rälspecifikation_typ
            {
              generalProperty = new typ
              {
                softType = _SoftTypeProperty,
                instanceRef = "typ"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              value = "?"
            }
            //TODO: Vikt + Rälprofil(Rälmodell) + hårdhet; Don't know where it comes from
          },
          företeelsetyp = new ClassificationReference_Anläggningsspecifikation_företeelsetyp
          {
            @class = new FTAnläggningsspecifikationReference
            {
              softType = "FTAnläggningsspecifikation",
              instanceRef = "Rälspecifikation"
            },
            startSpecified = false,
            endSpecified = false
          },
          id = "Rälspecifikation" + suffix
        };

        var rälprodukt = new Rälprodukt
        {
          name = "Rälprodukt",
          versionId = _VersionId,
          notering = bisRäl.Notering,
          numericSet = new RälproduktNumericSet
          {
            längd = SkapaLängd(bisRäl, new Rälprodukt_längd())
          },
          stringSet = new RälproduktStringSet
          {
            profiltyp = new Rälprodukt_profiltyp
            {
              generalProperty = new profiltyp
              {
                softType = _SoftTypeProperty,
                instanceRef = "profiltyp"
              },
              value = bisRäl.Rälmodell,
              JSonMapToPropertyName = _JsonMapToValue
            },
            stålsort = SkapaStålSort(bisRäl, new Rälprodukt_stålsort()),
            tillverkningsprocess = SkapaTillverkningsProcess(bisRäl, new Rälprodukt_tillverkningsprocess()),
            vikt = new Rälprodukt_vikt
            {
              generalProperty = new vikt
              {
                softType = _SoftTypeProperty,
                instanceRef = "vikt"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              value = bisRäl.Vikt
            }
          },
          företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTAnläggningsproduktReference
            {
              softType = "FTAnläggningsprodukt",
              instanceRef = "Rälprodukt"
            }
          },
          id = "Rälprodukt" + suffix
        };

        var rälindivid = new Rälindivid
        {
          startSpecified = false,
          endSpecified = false,
          versionId = _VersionId,
          notering = bisRäl.Notering,
          name = "Rälindivid",
          numericSet = new RälindividNumericSet(),
          stringSet = new RälindividStringSet
          {
            revideradKlassifikation = new Rälindivid_revideradKlassifikation
            {
              Array = true,
              generalProperty = new revideradKlassifikation
              {
                softType = _SoftTypeProperty,
                instanceRef = "revideradKlassifikation"
              },
              value = bisRäl.Rev_Klass
            }
          },
          företeelsetyp = new ClassificationReference_Bulkvara_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTBulkvaraReference
            {
              softType = "FTBulkvara",
              instanceRef = "Rälindivid"
            }
          },
          id = "Rälindivid" + suffix
        };

        //ENTRY INSTANCES
        var rälAnläggningsSpecifikation = new AnläggningsspecifikationEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "RälAnläggningsspecifikation" + suffix,
          data = rälSpec
        };
        rälAnläggningsSpecifikationer.Add(rälAnläggningsSpecifikation);

        var rälAnläggningsProdukt = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "RälAnläggningsprodukt" + suffix,
          data = rälprodukt
        };
        rälAnläggningsProdukter.Add(rälAnläggningsProdukt);

        var rälBulkvara = new BulkvaraEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "RälBulkvara" + suffix,
          data = rälindivid
        };
        rälBulkvaror.Add(rälBulkvara);

        ExtraCounter++;
      }

      //SOFTTYPES
      var anläggningsSpecifikationSoftType = new SoftType_Anläggningsspecifikation
      {
        Array = true,
        id = "Anläggningsspecifikation",
        instances = rälAnläggningsSpecifikationer.ToArray()
      };
      var anläggningsProduktSoftType = new SoftType_Anläggningsprodukt
      {
        Array = true,
        id = "Anläggningsprodukt",
        instances = rälAnläggningsProdukter.ToArray()
      };
      var bulkvaraSoftType = new SoftType_Bulkvara
      {
        Array = true,
        id = "Bulkvara",
        instances = rälBulkvaror.ToArray()
      };

      containerSoftypes.Add(anläggningsSpecifikationSoftType);
      containerSoftypes.Add(anläggningsProduktSoftType);
      containerSoftypes.Add(bulkvaraSoftType);
      containerSoftypes.AddRange(CreateSupplementarySoftypes());
      //New in this file
      containerSoftypes.AddRange(CreateFTKeyReferenceSoftTypes());
      // OLD BIG METHOD IN MAPPER THAT IS NOT USED ANYMORE
      //containerSoftypes.AddRange(CreateKeyReferences());

      container.softTypes = containerSoftypes.ToArray();
      return container;
    }

    public override List<SoftType> CreateFTKeyReferenceSoftTypes()
    {
      var softtypeList = new List<SoftType>();
      //FT A-Spec, A-Produkt och Bulkvara

      //FTAnläggningsProdukt
      var FTAnläggningsProdukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "Rälprodukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "Rälprodukt",
          name = "Läst från fil BIS_Räl - datadefinition infomod 2p0.xlsm"
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

      //FTAnläggningsspecifikation
      var FTAnläggningsspecifikationInstance = new FTAnläggningsspecifikationEntrydefaultIn
      {
        Array = true,
        id = "Rälspecifikation",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsspecifikationdefaultIn
        {
          id = "Rälspecifikation",
          name = "Läst från fil BIS_Räl - datadefinition infomod 2p0.xlsm"
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

      //FTBulkvara
      var FTBulkvaraInstance = new FTBulkvaraEntrydefaultIn
      {
        Array = true,
        id = "Rälindivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTBulkvaradefaultIn
        {
          id = "Rälindivid",
          name = "Läst från fil BIS_Räl - datadefinition infomod 2p0.xlsm"
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

      return softtypeList;
    }

    #region RälPropertyTranslators
    private Rälprodukt_tillverkningsprocess SkapaTillverkningsProcess(BIS_Räl bisRäl, Rälprodukt_tillverkningsprocess rälProcess)
    {
      rälProcess.generalProperty = new tillverkningsprocess
      {
        softType = _SoftTypeProperty,
        instanceRef = "tillverkningsprocess"
      };
      rälProcess.JSonMapToPropertyName = _JsonMapToValue;

      switch (bisRäl.Tillv_Process)
      {
        case "E":
          rälProcess.value = "Elektro";
          break;
        case "M":
          rälProcess.value = "Martin";
          break;
        case "S":
          rälProcess.value = "Syrgasprocess";
          break;
        case "T":
          rälProcess.value = "Thomas";
          break;
        case "?":
          rälProcess.value = "?";
          break;
      }
      return rälProcess;
    }

    private Rälprodukt_stålsort SkapaStålSort(BIS_Räl bisRäl, Rälprodukt_stålsort rälStålsort)
    {
      rälStålsort.generalProperty = new stålsort
      {
        softType = _SoftTypeProperty,
        instanceRef = "stålsort"
      };
      rälStålsort.JSonMapToPropertyName = _JsonMapToValue;

      switch (bisRäl.Stålsort)
      {
        case "1100":
          rälStålsort.value = "R320Cr";
          break;
        case "800":
          rälStålsort.value = "R220";
          break;
        case "900A":
          rälStålsort.value = "R260";
          break;
        case "Cr":
          rälStålsort.value = "R320Cr";
          break;
        case "HT":
          rälStålsort.value = "R350HT";
          break;
        case "LHT":
          rälStålsort.value = "R350LHT";
          break;
        default:
          rälStålsort.value = bisRäl.Stålsort;
          break;
      }
      return rälStålsort;
    }

    private Rälprodukt_längd SkapaLängd(BIS_Räl bisRäl, Rälprodukt_längd rälLängd)
    {
      rälLängd.generalProperty = new längd
      {
        softType = _SoftTypeProperty,
        instanceRef = "längd"
      };
      rälLängd.Array = true;
      rälLängd.JSonMapToPropertyName = _JsonMapToValue;
      rälLängd.Unit = new m
      {
        softType = "Unit",
        instanceRef = "m"
      };
      if (bisRäl.Längd != "?" && bisRäl.Längd != "L")
      {
        rälLängd.value = Convert.ToDecimal(bisRäl.Längd);
      }
      else
      {
        rälLängd.value = bisRäl.Längd == "L" ? 100 : 0;
      }
      return rälLängd;
    }

    private Rälspecifikation_skarvTyp SkapaSkarvTyp(BIS_Räl bisRäl, Rälspecifikation_skarvTyp rälSkarvTyp)
    {

      rälSkarvTyp.generalProperty = new skarvTyp
      {
        softType = _SoftTypeProperty,
        instanceRef = "skarvTyp"
      };
      rälSkarvTyp.JSonMapToPropertyName = _JsonMapToValue;

      switch (bisRäl.Skarvtyp)
      {
        case "T":
          rälSkarvTyp.value = "Treslipers (Skarvspår)";
          break;
        case "S":
          rälSkarvTyp.value = "Svävande (skarvspår)";
          break;
        case "L":
          rälSkarvTyp.value = "Långräl (Helsvetsat)";
          break;
        case "?":
          rälSkarvTyp.value = bisRäl.Skarvtyp;
          break;
      }
      return rälSkarvTyp;
    }
    #endregion
  }
}
