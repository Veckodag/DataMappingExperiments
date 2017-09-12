using System;
using System.Collections.Generic;
using System.Linq;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments.DataMapping
{
  internal class SkarvMapper : Mapper
  {
    public SkarvMapper()
    {
      MapperType = MapperType.Skarv;
      ExtraCounter = 1;
    }

    public sealed override MapperType MapperType { get; set; }

    public override BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt bisObject)
    {
      var mySkarv = (BIS_Skarv)bisObject;

      switch (index)
      {
        case 0:
          mySkarv.ObjektTypNummer = attributeValue;
          break;
        case 1:
          mySkarv.ObjektNummer = attributeValue;
          break;
        case 28:
          mySkarv.Skarvmodell = attributeValue;
          break;
        case 29:
          mySkarv.Dil_Skarv_serienr = attributeValue;
          break;
        case 30:
          mySkarv.Skarv_Tillv_år = attributeValue;
          break;
        case 32:
          mySkarv.isolerskarv_Passräl_längd = attributeValue;
          break;
        case 33:
          mySkarv.isolskarv_Hårdgjord = attributeValue;
          break;
        case 34:
          mySkarv.Isolskarv_DriftTillstånd = attributeValue;
          break;
        case 35:
          mySkarv.Besiktningsklass = attributeValue;
          break;
        case 36:
          mySkarv.Senast_ändrad = attributeValue;
          break;
        case 37:
          mySkarv.Senast_ändrad_av = attributeValue;
          break;
        case 38:
          mySkarv.Notering = attributeValue;
          break;
        case 39:
          mySkarv.Nivå = attributeValue;
          break;
        case 40:
          mySkarv.Komponent = attributeValue;
          break;
        case 41:
          mySkarv.Position = attributeValue;
          break;
        case 42:
          mySkarv.Modell = attributeValue;
          break;
        case 43:
          mySkarv.Art_nr = attributeValue;
          break;
        case 44:
          mySkarv.Ritnr = attributeValue;
          break;
        case 45:
          mySkarv.Material = attributeValue;
          break;
        case 46:
          mySkarv.Inldat = attributeValue;
          break;
        case 47:
          mySkarv.skarvavst_End_partikelm = attributeValue;
          break;
      }
      return mySkarv;
    }

    public override Container ObjectStructure(List<BIS_GrundObjekt> bisList)
    {
      Container container = new Container();
      var containerSofttypes = new List<SoftType>();

      var geografiskaplaceringsreferenser = new List<GeografiskPlaceringsreferensInstances>();
      var skarvFunktionellaAnläggningar = new List<FunktionellAnläggningInstances>();
      var skarvAnläggningsProdukter = new List<AnläggningsproduktInstances>();
      var skarvStyckevara = new List<StyckevaraInstances>();


      foreach (BIS_Skarv bisSkarv in bisList)
      {
        var suffix = bisSkarv.ObjektTypNummer + bisSkarv.ObjektNummer + ExtraCounter;

        //Noterings fix
        if (string.IsNullOrEmpty(bisSkarv.Notering))
        {
          bisSkarv.Notering = "Ingen notering";
        }

        var isolerskarvsprodukt = CreateIsolerSkarvsProdukt(bisSkarv, suffix);
        var isolerskarvsindivid = CreateIsolerSkarvsIndivid(bisSkarv, suffix);
        var partikelmagnetprodukt = CreatePartikelMagnetProdukt(bisSkarv, suffix);
        var partikelmagnetindivid = CreatePartikelMagnetIndivid(bisSkarv, suffix);
        var isolerskarv = CreateIsolerskarv(bisSkarv, suffix);

        //------------------------------------------------------------------------------Entry
        var isolerskarvEntry = new GeografiskPlaceringsreferensEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "Isolerskarv" + suffix,
          data = isolerskarv
        };
        geografiskaplaceringsreferenser.Add(isolerskarvEntry);

        var skarvAnläggningsProdukt = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "skarvAnläggningsProdukt" + suffix,
          data = isolerskarvsprodukt
        };
        skarvAnläggningsProdukter.Add(skarvAnläggningsProdukt);

        var isolerskarvsIndivid = new StyckevaraEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "IsolerskarvsIndivid" + suffix,
          data = isolerskarvsindivid
        };
        skarvStyckevara.Add(isolerskarvsIndivid);

        var partikelmagnetProduktEntry = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "Partikelmagnetprodukt" + suffix,
          data = partikelmagnetprodukt
        };
        skarvAnläggningsProdukter.Add(partikelmagnetProduktEntry);

        var styckevara = new StyckevaraEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "Partikelmagnetindivid" + suffix,
          data = partikelmagnetindivid
        };
        skarvStyckevara.Add(styckevara);

        ExtraCounter++;
      }

      //------------------------------------------------------------------------------SoftTypes
      var anläggningsProduktSoftType = new SoftType_Anläggningsprodukt
      {
        Array = true,
        id = "Anläggningsprodukt",
        instances = skarvAnläggningsProdukter.ToArray()
      };
      var styckSoftType = new SoftType_Styckevara
      {
        Array = true,
        id = "Styckevara",
        instances = skarvStyckevara.ToArray()
      };
      var funktionellSoftType = new SoftType_FunktionellAnläggning
      {
        Array = true,
        id = "Anläggningsfunktion",
        instances = skarvFunktionellaAnläggningar.ToArray()
      };
      var geografiskplaceringsreferenssofttype = new SoftType_GeografiskPlaceringsreferens
      {
        Array = true,
        id = "Anläggningsprodukt",
        instances = geografiskaplaceringsreferenser.ToArray()
      };

      containerSofttypes.Add(anläggningsProduktSoftType);
      containerSofttypes.Add(styckSoftType);
      //Använder inte funktionellAnläggning för tillfället
      containerSofttypes.Add(funktionellSoftType);
      containerSofttypes.Add(geografiskplaceringsreferenssofttype);
      containerSofttypes.AddRange(CreateFTKeyReferenceSoftTypes());

      container.softTypes = containerSofttypes.ToArray();
      return container;
    }

    public override List<SoftType> CreateFTKeyReferenceSoftTypes()
    {
      var softtypeList = new List<SoftType>();

      //FTGeografiskPlaceringsReferens
      var isolerskarv = new FTGeografiskPlaceringsreferensEntrydefaultIn
      {
        Array = true,
        id = "Isolerskarv",
        inputSchemaRef = _InputSchemaRef,
        data = new FTGeografiskPlaceringsreferensdefaultIn
        {
          id = "Isolerskarv",
          name = "Läst från fil BIS_Skarv - datadefinition infomod 2p0.xlsm"
        }
      };
      var FTGeografiskPlaceringsreferensInstances = new List<FTGeografiskPlaceringsreferensInstances>
      { isolerskarv };
      var FTGeografiskPlaceringsReferensSoftType = new SoftType_FTGeografiskPlaceringsreferens
      {
        Array = true,
        id = "FTGeografiskPlaceringsreferens",
        instances = FTGeografiskPlaceringsreferensInstances.ToArray()
      };
      softtypeList.Add(FTGeografiskPlaceringsReferensSoftType);
      //FTGeografiskPlaceringsReferens END

      //FTAnläggningsProdukt
      var isolerskarvProdukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "Isolerskarvprodukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "Isolerskarvprodukt",
          name = "Läst från fil BIS_Skarv - datadefinition infomod 2p0.xlsm"
        }
      };

      var partikelmagnetProdukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "Partikelmagnetprodukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "Partikelmagnetprodukt",
          name = "Läst från fil BIS_Skarv - datadefinition infomod 2p0.xlsm"
        }
      };

      var FTAnläggningsproduktInstances = new List<FTAnläggningsproduktInstances>
      { isolerskarvProdukt, partikelmagnetProdukt };
      var FTAnläggningsProduktSoftType = new SoftType_FTAnläggningsprodukt
      {
        Array = true,
        id = "FTAnläggningsprodukt",
        instances = FTAnläggningsproduktInstances.ToArray()
      };
      softtypeList.Add(FTAnläggningsProduktSoftType);
      //FTAnläggningsProdukt END

      //FTStyckevaror
      var isolerskarvindivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "Isolerskarvindivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "Isolerskarvindivid",
          name = "Läst från fil BIS_Skarv - datadefinition infomod 2p0.xlsm"
        }
      };

      var partikelmagnetIndivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "PartikelmagnetIndivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "PartikelmagnetIndivid",
          name = "Läst från fil BIS_Skarv - datadefinition infomod 2p0.xlsm"
        }
      };

      var FTStyckevaraInstances = new List<FTStyckevaraInstances>
      { isolerskarvindivid, partikelmagnetIndivid };
      var FTStyckevaraSoftType = new SoftType_FTStyckevara
      {
        Array = true,
        id = "FTStyckevara",
        instances = FTStyckevaraInstances.ToArray()
      };
      softtypeList.Add(FTStyckevaraSoftType);
      //FTStyckevara END
      return softtypeList;
    }

    private Isolerskarv CreateIsolerskarv(BIS_Skarv skarv, string suffix)
    {
      Isolerskarv isolerskarv = new Isolerskarv
      {
        name = "Isolerskarv",
        notering = skarv.Notering,
        versionId = _VersionId,
        företeelsetyp = new ClassificationReference_GeografiskPlaceringsreferens_företeelsetyp
        {
          @class = new FTGeografiskPlaceringsreferensReference
          {
            instanceRef = "Isolerskarv",
            softType = "FTGeografiskPlaceringsreferens"
          },
          startSpecified = false,
          endSpecified = false
        },
      };
      isolerskarv.id = isolerskarv.name + suffix;
      return isolerskarv;
    }
    private Isolerskarvprodukt CreateIsolerSkarvsProdukt(BIS_Skarv skarv, string suffix)
    {
      Isolerskarvprodukt isolerskarvsprodukt = new Isolerskarvprodukt
      {
        name = "Isolerskarvprodukt",
        notering = skarv.Notering,
        versionId = _VersionId,
        numericSet = new IsolerskarvproduktNumericSet
        {
          längdPassräl = new Isolerskarvprodukt_längdPassräl
          {
            value = CreateLength(skarv.isolerskarv_Passräl_längd),
            Array = true,
            generalProperty = new längdPassräl
            {
              instanceRef = "längdPassräl",
              softType = _SoftTypeProperty
            },
            JSonMapToPropertyName = _JsonMapToValue,
            Unit = new m
            {
              instanceRef = "m",
              softType = _SoftTypeUnit
            }
          }
        },
        stringSet = new IsolerskarvproduktStringSet
        {
          hårdgjord = new Isolerskarvprodukt_hårdgjord
          {
            generalProperty = new hårdgjord
            {
              instanceRef = "hårdgjord",
              softType = _SoftTypeProperty
            },
            JSonMapToPropertyName = _JsonMapToValue,
            value = skarv.isolskarv_Hårdgjord
          },
          typ = new Isolerskarvprodukt_typ
          {
            JSonMapToPropertyName = _JsonMapToValue,
            value = skarv.Skarvmodell,
            generalProperty = new typ
            {
              softType = _SoftTypeProperty,
              instanceRef = "typ"
            }
          }
        },
        företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
        {
          @class = new FTAnläggningsproduktReference
          {
            instanceRef = "Isolerskarvprodukt",
            softType = "FTAnläggningsprodukt"
          },
          startSpecified = false,
          endSpecified = false
        }
      };

      isolerskarvsprodukt.id = isolerskarvsprodukt.name + suffix;

      return isolerskarvsprodukt;
    }

    private Isolerskarvindivid CreateIsolerSkarvsIndivid(BIS_Skarv skarv, string suffix)
    {
      Isolerskarvindivid isolerskarvsindivid = new Isolerskarvindivid
      {
        name = "Isolerskarvsindivid",
        versionId = _VersionId,
        notering = skarv.Notering,
        företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
        {
          @class = new FTStyckevaraReference
          {
            instanceRef = "Isolerskarvindivid",
            softType = "FTStyckevara"
          },
          startSpecified = false,
          endSpecified = false
        },
        startSpecified = false,
        endSpecified = false
      };
      isolerskarvsindivid.id = isolerskarvsindivid.name + suffix;
      return isolerskarvsindivid;
    }

    private Partikelmagnetprodukt CreatePartikelMagnetProdukt(BIS_Skarv skarv, string suffix)
    {
      Partikelmagnetprodukt partikelmagnetprodukt = new Partikelmagnetprodukt
      {
        name = "Partikelmagnetprodukt",
        versionId = _VersionId,
        notering = skarv.Notering,
        företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
        {
          @class = new FTAnläggningsproduktReference
          {
            instanceRef = "Partikelmagnetprodukt",
            softType = "FTAnläggningsprodukt"
          },
          startSpecified = false,
          endSpecified = false
        }
      };
      partikelmagnetprodukt.id = partikelmagnetprodukt.name + suffix;
      return partikelmagnetprodukt;
    }

    private PartikelmagnetIndivid CreatePartikelMagnetIndivid(BIS_Skarv skarv, string suffix)
    {
      PartikelmagnetIndivid partikelmagnetindivid = new PartikelmagnetIndivid
      {
        name = "Partikelmagnetindivid",
        versionId = _VersionId,
        stringSet = new PartikelmagnetIndividStringSet
        {
          partikelmagnetposition = new PartikelmagnetIndivid_partikelmagnetposition
          {
            Array = true,
            value = string.IsNullOrEmpty(skarv.Position) ? "?" : skarv.Position,
            generalProperty = new partikelmagnetposition
            {
              instanceRef = "partikelmagnetposition",
              softType = _SoftTypeProperty
            },
            JSonMapToPropertyName = _JsonMapToValue
          }
        },
        numericSet = new PartikelmagnetIndividNumericSet
        {
          avståndFrånSkarv = new PartikelmagnetIndivid_avståndFrånSkarv
          {
            Array = true,
            Unit = new mm
            {
              instanceRef = "mm",
              softType = _SoftTypeUnit
            },
            value = CreateLength(skarv.skarvavst_End_partikelm),
            generalProperty = new avståndFrånSkarv
            {
              instanceRef = "avståndFrånSkarv",
              softType = _SoftTypeProperty
            },
            JSonMapToPropertyName = _JsonMapToValue
          }
        },
        företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
        {
          @class = new FTStyckevaraReference
          {
            instanceRef = "PartikelmagnetIndivid",
            softType = "FTStyckevara"
          },
          startSpecified = false,
          endSpecified = false
        }
      };
      partikelmagnetindivid.id = partikelmagnetindivid.name + suffix;

      return partikelmagnetindivid;
    }
  }
}
