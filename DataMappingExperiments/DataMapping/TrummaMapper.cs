using System;
using System.Collections.Generic;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments.DataMapping
{
  class TrummaMapper : Mapper
  {
    public TrummaMapper()
    {
      MapperType = MapperType.Trumma;
      FeatureTypeName = "Läst från fil BIS_Trumma-datadefinition infomod 2p0.xlsm";
    }

    public sealed override MapperType MapperType { get; set; }

    public override BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt bisObject)
    {
      var myTrumma = (BIS_Trumma)bisObject;
      switch (index)
      {
        case 0:
          myTrumma.ObjektTypNummer = attributeValue;
          break;
        case 1:
          myTrumma.ObjektNummer = attributeValue;
          break;
        case 29:
          myTrumma.UrsprungligtMaterial = attributeValue;
          break;
        case 30:
          myTrumma.Antalvattengångar = attributeValue;
          break;
        case 31:
          myTrumma.Ursprungligtyp = attributeValue;
          break;
        case 32:
          myTrumma.Infodring = attributeValue;
          break;
        case 33:
          myTrumma.DiamRörinfodring = attributeValue;
          break;
        case 34:
          myTrumma.FaunaTrumma = attributeValue;
          break;
        case 35:
          myTrumma.Bredd = attributeValue;
          break;
        case 36:
          myTrumma.Höjd = attributeValue;
          break;
        case 37:
          myTrumma.Diameter = attributeValue;
          break;
        case 38:
          myTrumma.Längd = attributeValue;
          break;
        case 39:
          myTrumma.FyllnadsHöjdDuruk = attributeValue;
          break;
        case 40:
          myTrumma.SläntlutningÖverstigande1_1_5 = attributeValue;
          break;
        case 41:
          myTrumma.ByggnadsÅr = attributeValue;
          break;
        case 42:
          myTrumma.MaterialFörlängningVänsterSida = attributeValue;
          break;
        case 43:
          myTrumma.MaterialFörlängningHögerSida = attributeValue;
          break;
        case 44:
          myTrumma.TypFörlängningVänsterSida = attributeValue;
          break;
        case 45:
          myTrumma.TypFörlängningHögerSida = attributeValue;
          break;
        case 46:
          myTrumma.DiameterFörlängningVänsterSida = attributeValue;
          break;
        case 47:
          myTrumma.DiameterFörlängningHögerSida = attributeValue;
          break;
        case 48:
          myTrumma.LängdFörlängningVänsterSida = attributeValue;
          break;
        case 49:
          myTrumma.LängdFörlängningHögerSida = attributeValue;
          break;
          //Case 50 är besiktningsklass, används inte
        case 51:
          myTrumma.Senast_Ändrad = attributeValue;
          break;
        case 52:
          myTrumma.Senast_Ändrad_Av = attributeValue;
          break;
        case 53:
          myTrumma.Notering = attributeValue;
          break;
      }
      return myTrumma;
    }

    public override Container ObjectStructure(List<BIS_GrundObjekt> bisList)
    {
      Container container = new Container();
      var containerSofttypes = new List<SoftType>();

      var anläggningsprodukter = new List<AnläggningsproduktInstances>();
      var funktionellaanläggningar = new List<FunktionellAnläggningInstances>();
      var styckevaror = new List<StyckevaraInstances>();

      foreach (BIS_Trumma bisTrumma in bisList)
      {
        var suffix = bisTrumma.ObjektTypNummer + bisTrumma.ObjektNummer + ExtraCounter;

        bisTrumma.Notering = NoteringsFix(bisTrumma.Notering);

        var trummaprodukt = CreateTrummaProdukt(bisTrumma, suffix);
        var trummafunktion = CreateTrummaFunktion(bisTrumma, suffix);
        var trummaindivid = CreateTrummaIndivid(bisTrumma, suffix);

        var trummafunktionEntry = new FunktionellAnläggningEntrydefaultIn()
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "trummafunktionEntry" + suffix,
          data = trummafunktion
        };
        funktionellaanläggningar.Add(trummafunktionEntry);

        var trummaproduktEntry = new AnläggningsproduktEntrydefaultIn()
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "trummaproduktEntry" + suffix,
          data = trummaprodukt
        };
        anläggningsprodukter.Add(trummaproduktEntry);

        var trummaindividEntry = new StyckevaraEntrydefaultIn()
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "trummaindividEntry" + suffix,
          data = trummaindivid
        };
        styckevaror.Add(trummaindividEntry);

        ExtraCounter++;
      }
      //SOFTTYPES
      var anläggningsproduktsofttype = new SoftType_Anläggningsprodukt
      {
        Array = true,
        id = "Anläggningsprodukt",
        instances = anläggningsprodukter.ToArray()
      };

      var funktionellanläggningsofttype = new SoftType_FunktionellAnläggning()
      {
        Array = true,
        id = "FunktionellAnläggning",
        instances = funktionellaanläggningar.ToArray()
      };

      var styckevarasofttype = new SoftType_Styckevara()
      {
        Array = true,
        id = "Styckevara",
        instances = styckevaror.ToArray()
      };

      containerSofttypes.Add(anläggningsproduktsofttype);
      containerSofttypes.Add(funktionellanläggningsofttype);
      containerSofttypes.Add(styckevarasofttype);
      containerSofttypes.AddRange(CreateSupplementarySoftypes());
      containerSofttypes.AddRange(CreateFTKeyReferenceSoftTypes());

      container.softTypes = containerSofttypes.ToArray();
      return container;
    }

    private Trummaprodukt CreateTrummaProdukt(BIS_Trumma p, string suffix)
    {
      Trummaprodukt o = new Trummaprodukt
      {
        name = "Trummaprodukt",
        versionId = _VersionId,
        numericSet = new TrummaproduktNumericSet
        {
          antalvattengångar = new Trummaprodukt_antalvattengångar
          {
            JSonMapToPropertyName = _JsonMapToValue,
            value = p.Antalvattengångar,
            generalProperty = new antalvattengångar
            {
              instanceRef = "antalvattengångar",
              softType = _SoftTypeProperty
            }
          },
          bredd = new Trummaprodukt_bredd
          {
            Unit = new m
            {
              softType = _SoftTypeUnit,
              instanceRef = "m"
            },
            generalProperty = new bredd
            {
              instanceRef = "bredd",
              softType = _SoftTypeProperty
            },
            value = DecimalConverter(p.Bredd),
            JSonMapToPropertyName = _JsonMapToValue
          },
          diamRörinfodring = new Trummaprodukt_diamRörinfodring
          {
            Unit = new m
            {
              softType = _SoftTypeUnit,
              instanceRef = "m"
            },
            generalProperty = new diamRörinfodring
            {
              instanceRef = "diamRörinfodring",
              softType = _SoftTypeProperty
            },
            value = p.DiamRörinfodring,
            JSonMapToPropertyName = _JsonMapToValue
          },
          diameter = new Trummaprodukt_diameter
          {
            Unit = new m
            {
              softType = _SoftTypeUnit,
              instanceRef = "m"
            },
            generalProperty = new diameter
            {
              instanceRef = "diameter",
              softType = _SoftTypeProperty
            },
            value = DecimalConverter(p.Diameter),
            JSonMapToPropertyName = _JsonMapToValue
          },
          diameterFörlängningHögerSida = new Trummaprodukt_diameterFörlängningHögerSida
          {
            Unit = new m
            {
              softType = _SoftTypeUnit,
              instanceRef = "m"
            },
            generalProperty = new diameterFörlängningHögerSida
            {
              instanceRef = "diameterFörlängningHögerSida",
              softType = _SoftTypeProperty
            },
            value = DecimalConverter(p.DiameterFörlängningHögerSida),
            JSonMapToPropertyName = _JsonMapToValue
          },
          diameterFörlängningVänsterSida = new Trummaprodukt_diameterFörlängningVänsterSida
          {
            Unit = new m
            {
              softType = _SoftTypeUnit,
              instanceRef = "m"
            },
            generalProperty = new diameterFörlängningVänsterSida
            {
              instanceRef = "diameterFörlängningVänsterSida",
              softType = _SoftTypeProperty
            },
            value = DecimalConverter(p.DiameterFörlängningVänsterSida),
            JSonMapToPropertyName = _JsonMapToValue
          },
          fyllnadshöjdURUK = new Trummaprodukt_fyllnadshöjdURUK
          {
            Unit = new m
            {
              softType = _SoftTypeUnit,
              instanceRef = "m"
            },
            generalProperty = new fyllnadshöjdURUK
            {
              instanceRef = "fyllnadshöjdURUK",
              softType = _SoftTypeProperty
            },
            value = DecimalConverter(p.FyllnadsHöjdDuruk),
            JSonMapToPropertyName = _JsonMapToValue
          },
          höjd = new Trummaprodukt_höjd
          {
            Unit = new m
            {
              softType = _SoftTypeUnit,
              instanceRef = "m"
            },
            generalProperty = new höjd
            {
              instanceRef = "höjd",
              softType = _SoftTypeProperty
            },
            value = DecimalConverter(p.Höjd),
            JSonMapToPropertyName = _JsonMapToValue
          },
          längd = new Trummaprodukt_längd
          {
            Unit = new m
            {
              softType = _SoftTypeUnit,
              instanceRef = "m"
            },
            generalProperty = new längd
            {
              instanceRef = "längd",
              softType = _SoftTypeProperty
            },
            value = DecimalConverter(p.Längd),
            JSonMapToPropertyName = _JsonMapToValue
          },
          längdFörlängningHögerSida = new Trummaprodukt_längdFörlängningHögerSida
          {
            Unit = new m
            {
              softType = _SoftTypeUnit,
              instanceRef = "m"
            },
            generalProperty = new längdFörlängningHögerSida
            {
              instanceRef = "längdFörlängningHögerSida",
              softType = _SoftTypeProperty
            },
            value = DecimalConverter(p.LängdFörlängningHögerSida),
            JSonMapToPropertyName = _JsonMapToValue
          },
          längdFörlängningVänsterSida = new Trummaprodukt_längdFörlängningVänsterSida
          {
            Unit = new m
            {
              softType = _SoftTypeUnit,
              instanceRef = "m"
            },
            generalProperty = new längdFörlängningVänsterSida
            {
              instanceRef = "längdFörlängningVänsterSida",
              softType = _SoftTypeProperty
            },
            value = DecimalConverter(p.LängdFörlängningVänsterSida),
            JSonMapToPropertyName = _JsonMapToValue
          }
        },
        notering = p.Notering,
        företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
        {
          @class = new FTAnläggningsproduktReference
          {
            softType = "FTAnläggningsprodukt",
            instanceRef = "Trummaprodukt"
          },
          endSpecified = false,
          startSpecified = false
        },
        stringSet = new TrummaproduktStringSet
        {
          infodring = new Trummaprodukt_infodring
          {
            value = p.Infodring,
            generalProperty = new infodring
            {
              instanceRef = "infodring",
              softType = _SoftTypeProperty
            },
            JSonMapToPropertyName = _JsonMapToValue
          },
          materialFörlängningHögerrSida = new Trummaprodukt_materialFörlängningHögerrSida
          {
            generalProperty = new materialFörlängningHögerrSida
            {
              softType = _SoftTypeProperty,
              instanceRef = "materialFörlängningHögerrSida"
            },
            JSonMapToPropertyName = _JsonMapToValue,
            value = p.MaterialFörlängningHögerSida
          },
          materialFörlängningVänsterSida = new Trummaprodukt_materialFörlängningVänsterSida
          {
            generalProperty = new materialFörlängningVänsterSida
            {
              instanceRef = "materialFörlängningVänsterSida",
              softType = _SoftTypeProperty
            },
            JSonMapToPropertyName = _JsonMapToValue,
            value = p.MaterialFörlängningVänsterSida,
          },
          släntlutningÖverstigande1_1_5 = new Trummaprodukt_släntlutningÖverstigande1_1_5
          {
            JSonMapToPropertyName = _JsonMapToValue,
            value = p.SläntlutningÖverstigande1_1_5,
            generalProperty = new släntlutningÖverstigande1_1_5
            {
              softType = _SoftTypeProperty,
              instanceRef = "släntlutningÖverstigande1_1_5"
            }
          },
          typFörlängningHögerSida = new Trummaprodukt_typFörlängningHögerSida
          {
            value = p.TypFörlängningHögerSida,
            generalProperty = new typFörlängningHögerSida
            {
              instanceRef = "typFörlängningHögerSida",
              softType = _SoftTypeProperty
            },
            JSonMapToPropertyName = _JsonMapToValue
          },
          typFörlängningVänsterSida = new Trummaprodukt_typFörlängningVänsterSida
          {
            generalProperty = new typFörlängningVänsterSida
            {
              softType = _SoftTypeProperty,
              instanceRef = "typFörlängningVänsterSida"
            },
            JSonMapToPropertyName = _JsonMapToValue,
            value = p.TypFörlängningVänsterSida
          },
          ursprungligTyp = new Trummaprodukt_ursprungligTyp
          {
            JSonMapToPropertyName = _JsonMapToValue,
            value = p.Ursprungligtyp,
            generalProperty = new ursprungligTyp
            {
              instanceRef = "ursprungligTyp",
              softType = _SoftTypeProperty
            }
          },
          ursprungligtMaterial = new Trummaprodukt_ursprungligtMaterial
          {
            value = p.UrsprungligtMaterial,
            generalProperty = new ursprungligtMaterial
            {
              softType = _SoftTypeProperty,
              instanceRef = "ursprungligtMaterial"
            },
            JSonMapToPropertyName = _JsonMapToValue
          }
        }
      };
      o.id = o.name + suffix;
      return o;
    }

    private Trummafunktion CreateTrummaFunktion(BIS_Trumma p, string suffix)
    {
      Trummafunktion o = new Trummafunktion
      {
        name = "Trummafunktion",
        versionId = _VersionId,
        stringSet = new TrummafunktionStringSet
        {
          faunatrumma = new Trummafunktion_faunatrumma
          {
            generalProperty = new faunatrumma
            {
              instanceRef = "faunatrumma",
              softType = _SoftTypeProperty
            },
            Array = true,
            JSonMapToPropertyName = _JsonMapToValue,
            value = p.FaunaTrumma
          }

        },
        företeelsetyp = new ClassificationReference_FunktionellAnläggning_företeelsetyp
        {
          @class = new FTFunktionellAnläggningReference
          {
            softType = "FTFunktionellAnläggning",
            instanceRef = "Trummafunktion"
          },
          startSpecified = false,
          endSpecified = false
        }
      };
      o.id = o.name + suffix;
      return o;

    }

    private Trummaindivid CreateTrummaIndivid(BIS_Trumma p, string suffix)
    {
      Trummaindivid o = new Trummaindivid
      {
        name = "Trummaindivid",
        versionId = _VersionId,
        företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
        {
          @class = new FTStyckevaraReference
          {
            softType = "FTStyckevara",
            instanceRef = "Trummaindivid"
          },
          startSpecified = false,
          endSpecified = false
        }
      };
      o.id = o.name + suffix;
      return o;
    }
    public override List<SoftType> CreateFTKeyReferenceSoftTypes()
    {
      var softtypeList = new List<SoftType>();

      //FTAnläggningsProdukt
      var trummaprodukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "Trummaprodukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "Trummaprodukt",
          name = FeatureTypeName
        }
      };
      var FTAnläggningsproduktInstances = new List<FTAnläggningsproduktInstances>
      { trummaprodukt };
      var FTAnläggningsProduktSoftType = new SoftType_FTAnläggningsprodukt
      {
        Array = true,
        id = "FTAnläggningsprodukt",
        instances = FTAnläggningsproduktInstances.ToArray()
      };
      softtypeList.Add(FTAnläggningsProduktSoftType);
      //FTAnläggningsProdukt END

      //FTStyckevara
      var trummaindivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "Trummaindivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "Trummaindivid",
          name = FeatureTypeName
        }
      };
      var FTStyckevaraInstances = new List<FTStyckevaraInstances>
      { trummaindivid };
      var FTStyckevaraSoftType = new SoftType_FTStyckevara
      {
        Array = true,
        id = "FTStyckevara",
        instances = FTStyckevaraInstances.ToArray()
      };
      softtypeList.Add(FTStyckevaraSoftType);
      //FTStyckevara END

      //FTFunktionellAnläggning
      var trummafunktion = new FTFunktionellAnläggningEntrydefaultIn
      {
        Array = true,
        id = "Trummafunktion",
        inputSchemaRef = _InputSchemaRef,
        data = new FTFunktionellAnläggningdefaultIn
        {
          id = "Trummafunktion",
          name = FeatureTypeName
        }
      };
      var FTFunktionellaAnläggningarInstances = new List<FTFunktionellAnläggningInstances>
      { trummafunktion };
      var FTFunktionellAnläggningSoftType = new SoftType_FTFunktionellAnläggning
      {
        Array = true,
        id = "FTFunktionellAnläggning",
        instances = FTFunktionellaAnläggningarInstances.ToArray()
      };
      softtypeList.Add(FTFunktionellAnläggningSoftType);
      //FTFunktionellAnläggning END

      return softtypeList;
    }
  }
}
