using System;
using System.Collections.Generic;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments.DataMapping
{
  public class SpårspärrMapper : Mapper
  {
    private readonly string _featureTypeName = "Läst från fil BIS_Spårspärr-datadefinition infomod 2p0_Arbete.xlsm";
    public SpårspärrMapper()
    {
      MapperType = MapperType.Spårspärr;
      ExtraCounter = 1;
    }
    public sealed override MapperType MapperType { get; set; }
    public override BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt bisObject)
    {
      var mySpårspärr = (BIS_SpårSpärr)bisObject;

      switch (index)
      {
        case 0:
          mySpårspärr.ObjektTypNummer = attributeValue;
          break;
        case 1:
          mySpårspärr.ObjektNummer = attributeValue;
          break;
        case 29:
          mySpårspärr.SpårspärrNummer = attributeValue;
          break;
        case 30:
          mySpårspärr.Modell = attributeValue;
          break;
        case 31:
          mySpårspärr.Besiktningsklass = attributeValue;
          break;
        case 32:
          mySpårspärr.Senast_Ändrad = attributeValue;
          break;
        case 33:
          mySpårspärr.Senast_Ändrad_Av = attributeValue;
          break;
        case 34:
          mySpårspärr.TLSUrsprung = attributeValue;
          break;
        case 35:
          mySpårspärr.Terminal = attributeValue;
          break;
        case 36:
          mySpårspärr.TLSId = attributeValue;
          break;
        case 37:
          mySpårspärr.TLSBeteckning = attributeValue;
          break;
        case 38:
          mySpårspärr.TLSTyp = attributeValue;
          break;
        case 39:
          mySpårspärr.CentralOmläggningsbar = attributeValue;
          break;
        case 40:
          mySpårspärr.GårAttSpärraIStällv = attributeValue;
          break;
        case 41:
          mySpårspärr.LokalfrigivBarIndivid = attributeValue;
          break;
        case 42:
          mySpårspärr.Återgående = attributeValue;
          break;
        case 43:
          mySpårspärr.FördFörÅtergång = attributeValue;
          break;
        //44 Not used
        case 45:
          mySpårspärr.Notering = attributeValue;
          break;
        case 46:
          mySpårspärr.Nivå = attributeValue;
          break;
        case 47:
          mySpårspärr.Komponent = attributeValue;
          break;
        case 48:
          mySpårspärr.Position = attributeValue;
          break;
        case 49:
          mySpårspärr.ModellDefinition = attributeValue;
          break;
        case 50:
          mySpårspärr.Artnr = attributeValue;
          break;
        case 51:
          mySpårspärr.Inldat = attributeValue;
          break;
      }

      return mySpårspärr;
    }

    public override Container ObjectStructure(List<BIS_GrundObjekt> bisList)
    {
      Container container = new Container();
      var containerSofttypes = new List<SoftType>();

      var geografiskaplaceringsreferenser = new List<GeografiskPlaceringsreferensInstances>();
      var anläggningsprodukter = new List<AnläggningsproduktInstances>();
      var funktionellaanläggningar = new List<FunktionellAnläggningInstances>();
      var styckevaror = new List<StyckevaraInstances>();

      foreach (BIS_SpårSpärr bisSpårSpärr in bisList)
      {
        var suffix = bisSpårSpärr.ObjektTypNummer + bisSpårSpärr.ObjektNummer + ExtraCounter;

        //Noterings fix
        if (string.IsNullOrEmpty(bisSpårSpärr.Notering))
          bisSpårSpärr.Notering = "Ingen notering";

        var spårspärr = CreateSpårSpärr(bisSpårSpärr, suffix);
        var funktionellspårspärr = CreateFunktionellSPårSpärr(bisSpårSpärr, suffix);
        var spårrspärrindivid = CreateSpårspärrindivid(bisSpårSpärr, suffix);
        var spårspärrprodukt = CreateSpårSpärrProdukt(bisSpårSpärr, suffix);
        var spårspärrklotsindivid = CreateSpårSpärrKlotIndivid(bisSpårSpärr, suffix);
        var spårspärrklotsprodukt = CreateSpårSpärrKlotProdukt(bisSpårSpärr, suffix);
        var spårspärrdrivindivid = CreateSpårSpärrDrivindivid(bisSpårSpärr, suffix);
        var spårspärrdrivprodukt = CreateSpårspärdrivProdukt(bisSpårSpärr, suffix);
        var elmotorindivid = CreateElmotorIndivid(bisSpårSpärr, suffix);
        var elmotorprodukt = CreateElmotorProdukt(bisSpårSpärr, suffix);
        var växelställindivid = CreateVäxelställindivid(bisSpårSpärr, suffix);
        var växelställprodukt = Createväxelställprodukt(bisSpårSpärr, suffix);
        var kontrollanordningsindivid = CreateKontrollanordningsindivid(bisSpårSpärr, suffix);
        var kontrollanordningsprodukt = Createkontrollannordningsprodukt(bisSpårSpärr, suffix);
        var staggropsvärmeindivid = CreateStaggropsvärmeindivid(bisSpårSpärr, suffix);
        var staggropsvärmeprodukt = CreateStaggropsvärmeprodukt(bisSpårSpärr, suffix);

        //ENTRY INSTANCEs      
        var spårspärrEntry = new GeografiskPlaceringsreferensEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "spårspärr" + suffix,
          data = spårspärr
        };
        geografiskaplaceringsreferenser.Add(spårspärrEntry);

        var funktionellspårspärrEntry = new FunktionellAnläggningEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "funktionellspårspärr" + suffix,
          data = funktionellspårspärr
        };
        funktionellaanläggningar.Add(funktionellspårspärrEntry);

        var spårspärrproduktEntry = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "spårspärrprodukt" + suffix,
          data = spårspärrprodukt
        };
        anläggningsprodukter.Add(spårspärrproduktEntry);

        var spårspärrklotsproduktEntry = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "spårspärrklotsprodukt" + suffix,
          data = spårspärrklotsprodukt
        };
        anläggningsprodukter.Add(spårspärrklotsproduktEntry);

        var spårspärrdrivproduktEntry = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "spårspärrdrivprodukt" + suffix,
          data = spårspärrdrivprodukt
        };
        anläggningsprodukter.Add(spårspärrdrivproduktEntry);

        var elmotorproduktEntry = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "elmotorprodukt" + suffix,
          data = elmotorprodukt
        };
        anläggningsprodukter.Add(elmotorproduktEntry);

        var växelställproduktEntry = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "växelställprodukt" + suffix,
          data = växelställprodukt
        };
        anläggningsprodukter.Add(växelställproduktEntry);

        var kontrollanordningsproduktEntry = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "kontrollanordningsprodukt" + suffix,
          data = kontrollanordningsprodukt
        };
        anläggningsprodukter.Add(kontrollanordningsproduktEntry);

        var staggropsvärmeproduktEntry = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "staggropsvärmeprodukt" + suffix,
          data = staggropsvärmeprodukt
        };
        anläggningsprodukter.Add(staggropsvärmeproduktEntry);

        var spårspärrklotsindividEntry = new StyckevaraEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "spårspärrklotsindivid" + suffix,
          data = spårspärrklotsindivid
        };
        styckevaror.Add(spårspärrklotsindividEntry);


        var spårrspärrindividEntry = new StyckevaraEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "spårrspärrindivid" + suffix,
          data = spårrspärrindivid
        };
        styckevaror.Add(spårrspärrindividEntry);

        var spårspärrdrivindividEntry = new StyckevaraEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "spårspärrdrivindivid" + suffix,
          data = spårspärrdrivindivid
        };
        styckevaror.Add(spårspärrdrivindividEntry);

        var elmotorindividEntry = new StyckevaraEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "elmotorindivid" + suffix,
          data = elmotorindivid
        };
        styckevaror.Add(elmotorindividEntry);

        var växelställindividEntry = new StyckevaraEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "växelställindivid" + suffix,
          data = växelställindivid
        };
        styckevaror.Add(växelställindividEntry);

        var kontrollanordningsindividEntry = new StyckevaraEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "kontrollanordningsindivid" + suffix,
          data = kontrollanordningsindivid
        };
        styckevaror.Add(kontrollanordningsindividEntry);

        var staggropsvärmeindividEntry = new StyckevaraEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "staggropsvärmeindivid" + suffix,
          data = staggropsvärmeindivid
        };
        styckevaror.Add(staggropsvärmeindividEntry);

        ExtraCounter++;
      }

      //SOFTTYPES
      var anläggningsproduktsofttype = new SoftType_Anläggningsprodukt
      {
        Array = true,
        id = "Anläggningsprodukt",
        instances = anläggningsprodukter.ToArray()
      };

      var funktionellanläggningsofttype = new SoftType_FunktionellAnläggning
      {
        Array = true,
        id = "FunktionellAnläggning",
        instances = funktionellaanläggningar.ToArray()
      };

      var styckevarasofttype = new SoftType_Styckevara
      {
        Array = true,
        id = "Styckevara",
        instances = styckevaror.ToArray()
      };

      var geografiskplaceringsreferenssofttype = new SoftType_GeografiskPlaceringsreferens
      {
        Array = true,
        id = "GeografiskPlaceringsreferens",
        instances = geografiskaplaceringsreferenser.ToArray()
      };

      containerSofttypes.Add(anläggningsproduktsofttype);
      containerSofttypes.Add(funktionellanläggningsofttype);
      containerSofttypes.Add(styckevarasofttype);
      containerSofttypes.Add(geografiskplaceringsreferenssofttype);

      container.softTypes = containerSofttypes.ToArray();

      return container;
    }

    public override List<SoftType> CreateFTKeyReferenceSoftTypes()
    {
      var softtypeList = new List<SoftType>();

      //FTGeografiskPlaceringsReferens
      var spårspärr = new FTGeografiskPlaceringsreferensEntrydefaultIn
      {
        Array = true,
        id = "Spårspärr",
        inputSchemaRef = _InputSchemaRef,
        data = new FTGeografiskPlaceringsreferensdefaultIn
        {
          id = "Spårspärr",
          name = _featureTypeName
        }
      };
      var FTGeografiskPlaceringsreferensInstances = new List<FTGeografiskPlaceringsreferensInstances>
      { spårspärr };
      var FTGeografiskPlaceringsReferensSoftType = new SoftType_FTGeografiskPlaceringsreferens
      {
        Array = true,
        id = "FTGeografiskPlaceringsreferens",
        instances = FTGeografiskPlaceringsreferensInstances.ToArray()
      };
      softtypeList.Add(FTGeografiskPlaceringsReferensSoftType);

      //FTFunktionellAnläggning
      var funktionellSpårspärr = new FTFunktionellAnläggningEntrydefaultIn
      {
        Array = true,
        id = "FunktionellSpårspärr",
        inputSchemaRef = _InputSchemaRef,
        data = new FTFunktionellAnläggningdefaultIn
        {
          id = "FunktionellSpårspärr",
          name = _featureTypeName
        }
      };

      var FTFunktionellAnläggningInstances = new List<FTFunktionellAnläggningInstances>
      { funktionellSpårspärr };
      var FTFunktionellAnläggningSoftType = new SoftType_FTFunktionellAnläggning
      {
        Array = true,
        id = "FTFunktionellAnläggning",
        instances = FTFunktionellAnläggningInstances.ToArray()
      };
      softtypeList.Add(FTFunktionellAnläggningSoftType);

      //FTStyckevara
      var spårspärrindivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "Spårspärrindivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "Spårspärrindivid",
          name = _featureTypeName
        }
      };

      var spårspärrklotsindivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "Spårspärrklotsindivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "Spårspärrklotsindivid",
          name = _featureTypeName
        }
      };

      var spårspärrdrivindivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "Spårspärrdrivindivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "Spårspärrdrivindivid",
          name = _featureTypeName
        }
      };

      var elmotorindivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "Elmotorindivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "Elmotorindivid",
          name = _featureTypeName
        }
      };

      var växelställindivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "Växelställindivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "Växelställindivid",
          name = _featureTypeName
        }
      };

      var kontrollanordningsindivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "Kontrollanordningsindivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "Kontrollanordningsindivid",
          name = _featureTypeName
        }
      };

      var staggropsvärmeindivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "Staggropsvärmeindivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "Staggropsvärmeindivid",
          name = _featureTypeName
        }
      };

      var FTStyckevaraInstances = new List<FTStyckevaraInstances>
      { spårspärrindivid, spårspärrklotsindivid, spårspärrdrivindivid,
        elmotorindivid, växelställindivid, kontrollanordningsindivid,
        staggropsvärmeindivid };
      var FTStyckevaraSoftType = new SoftType_FTStyckevara
      {
        Array = true,
        id = "FTStyckevara",
        instances = FTStyckevaraInstances.ToArray()
      };
      softtypeList.Add(FTStyckevaraSoftType);

      //FTAnläggningsProdukt
      var spårspärrprodukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "Spårspärrprodukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "Spårspärrprodukt",
          name = _featureTypeName
        }
      };

      var spårspärrklotsprodukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "Spårspärrklotsprodukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "Spårspärrklotsprodukt",
          name = _featureTypeName
        }
      };

      var spårspärrdrivprodukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "Spårspärrdrivprodukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "Spårspärrdrivprodukt",
          name = _featureTypeName
        }
      };
      var elmotorprodukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "Elmotorprodukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "Elmotorprodukt",
          name = _featureTypeName
        }
      };
      var växelställprodukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "Växelställprodukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "Växelställprodukt",
          name = _featureTypeName
        }
      };
      var kontrollanordningsprodukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "Kontrollanordningsprodukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "Kontrollanordningsprodukt",
          name = _featureTypeName
        }
      };

      var staggropsvärmeprodukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "Staggropsvärmeprodukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "Staggropsvärmeprodukt",
          name = _featureTypeName
        }
      };
      var FTAnläggningsproduktInstances = new List<FTAnläggningsproduktInstances>
      { spårspärrprodukt, spårspärrklotsprodukt, spårspärrdrivprodukt, elmotorprodukt,
        växelställprodukt, kontrollanordningsprodukt, staggropsvärmeprodukt };
      var FTAnläggningsProduktSoftType = new SoftType_FTAnläggningsprodukt
      {
        Array = true,
        id = "FTAnläggningsprodukt",
        instances = FTAnläggningsproduktInstances.ToArray()
      };
      softtypeList.Add(FTAnläggningsProduktSoftType);

      return softtypeList;
    }
    private Spårspärr CreateSpårSpärr(BIS_SpårSpärr p, string suffix)
    {
      Spårspärr spårspärr = new Spårspärr
      {
        name = "Spårspärr",
        notering = p.Notering,
        versionId = _VersionId,
        stringSet = new SpårspärrStringSet
        {
          IDICONIS = new Spårspärr_IDICONIS
          {
            value = "?",
            JSonMapToPropertyName = _JsonMapToValue,
            generalProperty = new IDICONIS
            {
              instanceRef = "ID-ICONIS",
              softType = _SoftTypeProperty
            }
          },
          TLSbeteckning = new Spårspärr_TLSbeteckning
          {
            value = string.IsNullOrEmpty(p.TLSBeteckning) ? "?" : p.TLSBeteckning,
            JSonMapToPropertyName = _JsonMapToValue,
            generalProperty = new TLSbeteckning
            {
              instanceRef = "TLS-beteckning",
              softType = _SoftTypeProperty
            }
          },
          TLSid = new Spårspärr_TLSid
          {
            value = string.IsNullOrEmpty(p.TLSId) ? "?" : p.TLSId,
            generalProperty = new TLSid
            {
              instanceRef = "TLS-id",
              softType = _SoftTypeProperty
            },
            JSonMapToPropertyName = _JsonMapToValue
          },
          TLSterminal = new Spårspärr_TLSterminal
          {
            value = string.IsNullOrEmpty(p.Terminal) ? "?" : p.Terminal,
            generalProperty = new TLSterminal
            {
              instanceRef = "TLS-terminal",
              softType = _SoftTypeProperty
            },
            JSonMapToPropertyName = _JsonMapToValue
          },
          TLStyp = new Spårspärr_TLStyp
          {
            value = string.IsNullOrEmpty(p.TLSTyp) ? "?" : p.TLSTyp,
            generalProperty = new TLStyp
            {
              instanceRef = "TLS-typ",
              softType = _SoftTypeProperty
            },
            JSonMapToPropertyName = _JsonMapToValue
          },
          TLSursprung = new Spårspärr_TLSursprung
          {
            value = string.IsNullOrEmpty(p.TLSUrsprung) ? "?" : p.TLSUrsprung,
            generalProperty = new TLSursprung
            {
              instanceRef = "TLS-ursprung",
              softType = _SoftTypeProperty
            },
            JSonMapToPropertyName = _JsonMapToValue
          },
          spårspärrNr = new Spårspärr_spårspärrNr
          {
            value = p.SpårspärrNummer,
            generalProperty = new spårspärrNr
            {
              instanceRef = "spårspärrNr",
              softType = _SoftTypeProperty
            },
            JSonMapToPropertyName = _JsonMapToValue
          }
        },
        företeelsetyp = new ClassificationReference_GeografiskPlaceringsreferens_företeelsetyp
        {
          @class = new FTGeografiskPlaceringsreferensReference
          {
            instanceRef = "Spårspärr",
            softType = "FTGeografiskPlaceringsreferens"
          },
          startSpecified = false,
          endSpecified = false
        }
      };

      spårspärr.id = spårspärr.name + suffix;
      return spårspärr;
    }
    private FunktionellSpårspärr CreateFunktionellSPårSpärr(BIS_SpårSpärr p, string suffix)
    {
      FunktionellSpårspärr funktionelspårspärr = new FunktionellSpårspärr
      {
        name = "FunktionellSpårSpärr",
        notering = p.Notering,
        versionId = _VersionId,
        stringSet = new FunktionellSpårspärrStringSet
        {
          Centraltomläggningsbar = new FunktionellSpårspärr_Centraltomläggningsbar
          {
            value = HandleEmtyStringValue(p.CentralOmläggningsbar),
            generalProperty = new Centraltomläggningsbar
            {
              instanceRef = "Centralt_x0020_omläggningsbar",
              softType = _SoftTypeProperty
            },
            JSonMapToPropertyName = _JsonMapToValue
          },
          Gårattspärraiställverk = new FunktionellSpårspärr_Gårattspärraiställverk
          {
            value = HandleEmtyStringValue(p.GårAttSpärraIStällv),
            generalProperty = new Gårattspärraiställverk
            {
              instanceRef = "Går_x0020_att_x0020_spärra_x0020_i_x0020_ställverk",
              softType = _SoftTypeProperty
            },
            JSonMapToPropertyName = _JsonMapToValue
          },
          Lokalfrigivningsbarindividuellt = new FunktionellSpårspärr_Lokalfrigivningsbarindividuellt
          {
            value = HandleEmtyStringValue(p.LokalfrigivBarIndivid),
            generalProperty = new Lokalfrigivningsbarindividuellt
            {
              instanceRef = "Lokalfrigivningsbar_x0020_individuellt",
              softType = _SoftTypeProperty
            },
            JSonMapToPropertyName = _JsonMapToValue
          },
          Återgående = new FunktionellSpårspärr_Återgående
          {
            value = HandleEmtyStringValue(p.Återgående),
            generalProperty = new Återgående
            {
              instanceRef = "Återgående",
              softType = _SoftTypeProperty
            },
            JSonMapToPropertyName = _JsonMapToValue
          }
        },
        företeelsetyp = new ClassificationReference_FunktionellAnläggning_företeelsetyp
        {
          @class = new FTFunktionellAnläggningReference
          {
            instanceRef = "FunktionellSpårspärr",
            softType = "FTFunktionellAnläggning"
          },
          startSpecified = false,
          endSpecified = false
        }
      };

      funktionelspårspärr.id = funktionelspårspärr.name + suffix;
      return funktionelspårspärr;
    }
    private Spårspärrindivid CreateSpårspärrindivid(BIS_SpårSpärr p, string suffix)
    {
      Spårspärrindivid spårspärrindivid = new Spårspärrindivid
      {
        name = "Spårspärrindivid",
        notering = p.Notering,
        versionId = _VersionId,
        företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
        {
          @class = new FTStyckevaraReference
          {
            instanceRef = "Spårspärrindivid",
            softType = "FTStyckevara"
          },
          endSpecified = false,
          startSpecified = false
        }
      };

      spårspärrindivid.id = spårspärrindivid.name + suffix;
      return spårspärrindivid;
    }
    private Spårspärrprodukt CreateSpårSpärrProdukt(BIS_SpårSpärr p, string suffix)
    {
      Spårspärrprodukt spårspärrprodukt = new Spårspärrprodukt
      {
        name = "Spårspärrprodukt",
        notering = p.Notering,
        versionId = _VersionId,
        stringSet = new SpårspärrproduktStringSet
        {
          Typavspårspärr = new Spårspärrprodukt_Typavspårspärr
          {
            Array = true,
            value = p.Modell,
            generalProperty = new Typavspårspärr
            {
              instanceRef = "Typ_x0020_av_x0020_spårspärr",
              softType = _SoftTypeProperty
            },
            JSonMapToPropertyName = _JsonMapToValue
          }
        },
        företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
        {
          @class = new FTAnläggningsproduktReference
          {
            instanceRef = "Spårspärrprodukt",
            softType = "FTAnläggningsprodukt"
          },
          startSpecified = false,
          endSpecified = false
        }
      };

      spårspärrprodukt.id = spårspärrprodukt.name + suffix;
      return spårspärrprodukt;
    }
    private Spårspärrklotsindivid CreateSpårSpärrKlotIndivid(BIS_SpårSpärr p, string suffix)
    {
      Spårspärrklotsindivid spårspärrklotindivid = new Spårspärrklotsindivid
      {
        name = "Spårspärrklotindivid",
        notering = p.Notering,
        versionId = _VersionId,
        företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
        {
          @class = new FTStyckevaraReference
          {
            instanceRef = "Spårspärrklotsindivid",
            softType = "FTStyckevara"
          },
          startSpecified = false,
          endSpecified = false
        }
      };
      spårspärrklotindivid.id = spårspärrklotindivid.name + suffix;
      return spårspärrklotindivid;
    }
    private Spårspärrklotsprodukt CreateSpårSpärrKlotProdukt(BIS_SpårSpärr p, string suffix)
    {
      Spårspärrklotsprodukt spårspärrklotprodukt = new Spårspärrklotsprodukt
      {
        name = "Spårspärrklotsprodukt",
        notering = p.Notering,
        versionId = _VersionId,
        företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
        {
          @class = new FTAnläggningsproduktReference
          {
            instanceRef = "Spårspärrklotsprodukt",
            softType = "FTAnläggningsprodukt"
          },
          startSpecified = false,
          endSpecified = false
        }
      };

      spårspärrklotprodukt.id = spårspärrklotprodukt.name + suffix;
      return spårspärrklotprodukt;
    }
    private Spårspärrdrivindivid CreateSpårSpärrDrivindivid(BIS_SpårSpärr p, string suffix)
    {
      Spårspärrdrivindivid o = new Spårspärrdrivindivid
      {
        name = "Spårspärrsdrivindivid",
        notering = p.Notering,
        versionId = _VersionId,
        företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
        {
          @class = new FTStyckevaraReference
          {
            instanceRef = "Spårspärrdrivindivid",
            softType = "FTStyckevara"
          },
          startSpecified = false,
          endSpecified = false
        }
      };
      o.id = o.name + suffix;
      return o;
    }
    private Spårspärrdrivprodukt CreateSpårspärdrivProdukt(BIS_SpårSpärr p, string suffix)
    {
      Spårspärrdrivprodukt o = new Spårspärrdrivprodukt
      {
        name = "Spårspärrdrivprodukt",
        notering = p.Notering,
        versionId = _VersionId,
        företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
        {
          @class = new FTAnläggningsproduktReference
          {
            instanceRef = "Spårspärrdrivprodukt",
            softType = "FTAnläggningsprodukt"
          },
          startSpecified = false,
          endSpecified = false
        }
      };
      o.id = o.name + suffix;
      return o;
    }
    private Elmotorindivid CreateElmotorIndivid(BIS_SpårSpärr p, string suffix)
    {
      Elmotorindivid o = new Elmotorindivid
      {
        name = "Elmotorindivid",
        notering = p.Notering,
        versionId = _VersionId,
        företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
        {
          @class = new FTStyckevaraReference
          {
            instanceRef = "Elmotorindivid",
            softType = "FTStyckevara"
          },
          startSpecified = false,
          endSpecified = false
        }
      };
      o.id = o.name + suffix;
      return o;
    }

    private Elmotorprodukt CreateElmotorProdukt(BIS_SpårSpärr p, string suffix)
    {
      Elmotorprodukt o = new Elmotorprodukt
      {
        name = "Elmotorprodukt",
        notering = p.Notering,
        versionId = _VersionId,
        företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
        {
          @class = new FTAnläggningsproduktReference
          {
            instanceRef = "Elmotorprodukt",
            softType = "FTAnläggningsprodukt"
          },
          startSpecified = false,
          endSpecified = false
        }
      };
      o.id = o.name + suffix;
      return o;
    }
    private Växelställindivid CreateVäxelställindivid(BIS_SpårSpärr p, string suffix)
    {
      Växelställindivid o = new Växelställindivid
      {
        name = "Växelställindivid",
        notering = p.Notering,
        versionId = _VersionId,
        företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
        {
          @class = new FTStyckevaraReference
          {
            instanceRef = "Växelställindivid",
            softType = "FTStyckevara"
          },
          startSpecified = false,
          endSpecified = false
        }
      };
      o.id = o.name + suffix;
      return o;
    }
    private Växelställprodukt Createväxelställprodukt(BIS_SpårSpärr p, string suffix)
    {
      Växelställprodukt o = new Växelställprodukt
      {
        name = "Växelställprodukt",
        notering = p.Notering,
        versionId = _VersionId,
        företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
        {
          @class = new FTAnläggningsproduktReference
          {
            instanceRef = "Växelställprodukt",
            softType = "FTAnläggningsprodukt"
          },
          startSpecified = false,
          endSpecified = false
        }
      };
      o.id = o.name + suffix;
      return o;
    }
    private Kontrollanordningsindivid CreateKontrollanordningsindivid(BIS_SpårSpärr p, string suffix)
    {
      Kontrollanordningsindivid o = new Kontrollanordningsindivid
      {
        name = "Kontrollannordningsindivid",
        notering = p.Notering,
        versionId = _VersionId,
        företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
        {
          @class = new FTStyckevaraReference
          {
            instanceRef = "Kontrollanordningsindivid",
            softType = "FTStyckevara"
          },
          startSpecified = false,
          endSpecified = false
        }
      };
      o.id = o.name + suffix;
      return o;
    }
    private Kontrollanordningsprodukt Createkontrollannordningsprodukt(BIS_SpårSpärr p, string suffix)
    {
      Kontrollanordningsprodukt o = new Kontrollanordningsprodukt
      {
        name = "Kontrollannordningsprodukt",
        notering = p.Notering,
        versionId = _VersionId,
        företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
        {
          @class = new FTAnläggningsproduktReference
          {
            instanceRef = "Kontrollanordningsprodukt",
            softType = "FTAnläggningsprodukt"
          },
          startSpecified = false,
          endSpecified = false
        }
      };
      o.id = o.name + suffix;
      return o;
    }
    private Staggropsvärmeindivid CreateStaggropsvärmeindivid(BIS_SpårSpärr p, string suffix)
    {
      Staggropsvärmeindivid o = new Staggropsvärmeindivid
      {
        name = "Staggropsvärmeindivid",
        notering = p.Notering,
        versionId = _VersionId,
        företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
        {
          @class = new FTStyckevaraReference
          {
            instanceRef = "Staggropsvärmeindivid",
            softType = "FTStyckevara"
          },
          startSpecified = false,
          endSpecified = false
        }
      };
      o.id = o.name + suffix;
      return o;
    }
    private Staggropsvärmeprodukt CreateStaggropsvärmeprodukt(BIS_SpårSpärr p, string suffix)
    {
      Staggropsvärmeprodukt o = new Staggropsvärmeprodukt
      {
        name = "Staggropsvärmeprodukt",
        notering = p.Notering,
        versionId = _VersionId,
        företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
        {
          @class = new FTAnläggningsproduktReference
          {
            instanceRef = "Staggropsvärmeprodukt",
            softType = "FTAnläggningsprodukt"
          },
          startSpecified = false,
          endSpecified = false
        }
      };
      o.id = o.name + suffix;
      return o;
    }
    private string HandleEmtyStringValue(string value)
    {
      if (value == string.Empty)
        value = "-";

      return value;
    }
  }
}
