﻿using System;
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
      ExtraCounter = 1;
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
        //Allt nytt
        var rälSpec = new Rälspecifikation
        {
          name = "Rälspecifikation",
          notering = bisRäl.Notering,
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
              //Vikt + Rälprofil(Rälmodell) + hårdhet; Don't know where it comes from
              value = null,
              JSonMapToPropertyName = _JsonMapToValue
            }
          },
          företeelsetyp = new ClassificationReference_Anläggningsspecifikation_företeelsetyp
          {
            @class = new FTAnläggningsspecifikationReference
            {
              softType = "FTAnläggningsspecifikation",
              instanceRef = "FTAnläggningsspecifikation"
            },
            startSpecified = false,
            endSpecified = false
          }
        };
        rälSpec.id = rälSpec.name + suffix;

        var rälprodukt = new Rälprodukt
        {
          name = "RälProdukt",
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
              instanceRef = "FTAnläggningsprodukt"
            }
          }
        };
        rälprodukt.id = rälprodukt.name + suffix;

        var rälindivid = new Rälindivid
        {
          startSpecified = false,
          endSpecified = false,
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
              instanceRef = "FTBulkvara"
            }
          }
        };
        rälindivid.id = rälindivid.name + suffix;

        //ENTRY INSTANCES
        var rälAnläggningsSpecifikation = new AnläggningsspecifikationEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "RälAnläggningsspecifikationEntrydefaultIn" + suffix,
          data = rälSpec
        };
        rälAnläggningsSpecifikationer.Add(rälAnläggningsSpecifikation);

        var rälAnläggningsProdukt = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "RälAnläggningsproduktEntrydefaultIn" + suffix,
          data = rälprodukt
        };
        rälAnläggningsProdukter.Add(rälAnläggningsProdukt);

        var rälBulkvara = new BulkvaraEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "RälBulkvaraEntrydefaultIn" + suffix,
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
      containerSoftypes.AddRange(CreateKeyReferences());

      container.softTypes = containerSoftypes.ToArray();
      return container;
    }

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

    /// <summary>
    /// Squashing of the list. Unika plattformar: utan versioner med olika nätanknytningar.
    /// </summary>
    /// <param name="bisList"></param>
    /// <returns></returns>
    public override IEnumerable<BIS_GrundObjekt> SquashTheList(List<BIS_GrundObjekt> bisList)
    {
      var myList = new List<BIS_Räl>();

      foreach (var objekt in bisList)
        myList.Add(objekt as BIS_Räl);

      //Kommer att trycka ihop listan på objektnummer
      return myList.GroupBy(objektDetalj => objektDetalj.ObjektNummer)
        .Select(values => values.FirstOrDefault()).ToList();
    }
  }
}
