using System;
using System.Collections.Generic;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments.DataMapping
{
  public class TeknikbyggnadMapper : Mapper
  {
    public TeknikbyggnadMapper()
    {
      MapperType = MapperType.Teknikbyggnad;
      FeatureTypeName = "Läst från fil BIS_Teknikbyggnad - datadefinition infomod 2p0.xlsm";
    }
    public sealed override MapperType MapperType { get; set; }
    //private bool _IsComponent = false;
    public override BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt bisObject)
    {
      var myTeknikByggnad = (BIS_Teknikbyggnad)bisObject;

      switch (index)
      {
        case 0:
          myTeknikByggnad.ObjektTypNummer = attributeValue;
          break;
        case 1:
          myTeknikByggnad.ObjektNummer = attributeValue;
          break;
        case 24:
          myTeknikByggnad.Ägare = attributeValue;
          break;
        case 28:
          myTeknikByggnad.Modell = attributeValue;
          break;
        case 29:
          myTeknikByggnad.Beteckning = attributeValue;
          break;
        case 30:
          myTeknikByggnad.ByggnadsÅr = attributeValue;
          break;
        case 31:
          myTeknikByggnad.Typ = attributeValue;
          break;
        case 32:
          myTeknikByggnad.Fasadutförande = attributeValue;
          break;
        case 33:
          myTeknikByggnad.Åskskydd = attributeValue;
          break;
        case 34:
          myTeknikByggnad.OrdinarieNät = attributeValue;
          break;
        case 35:
          myTeknikByggnad.RedundantNät = attributeValue;
          break;
        case 36:
          myTeknikByggnad.HjälpkraftFaser = attributeValue;
          break;
        case 37:
          myTeknikByggnad.HjälpkraftSäkring = attributeValue;
          break;
        case 38:
          myTeknikByggnad.OrtsnätFaser = attributeValue;
          break;
        case 39:
          myTeknikByggnad.OrtsnätSäkring = attributeValue;
          break;
        case 40:
          myTeknikByggnad.OrtsnätIDNummer = attributeValue;
          break;
        case 41:
          myTeknikByggnad.OrtsnätÄgare = attributeValue;
          break;
        case 42:
          myTeknikByggnad.MellantrafoStorlek = attributeValue;
          break;
        case 43:
          myTeknikByggnad.Sidoavstånd = attributeValue;
          break;
        case 44:
          myTeknikByggnad.Northing = attributeValue;
          break;
        case 45:
          myTeknikByggnad.Easting = attributeValue;
          break;
        case 46:
          myTeknikByggnad.ReservelverkUtförande = attributeValue;
          break;
        case 47:
          myTeknikByggnad.ReservelverkStorlek = attributeValue;
          break;
        case 48:
          myTeknikByggnad.ReservelverkInstÅr = attributeValue;
          break;
        case 49:
          myTeknikByggnad.ReservelverkTankvolym = attributeValue;
          break;
        case 50:
          myTeknikByggnad.ReservelverkBatterimodell = attributeValue;
          break;
        case 51:
          myTeknikByggnad.ReserveelverkBatteriInDatum = attributeValue;
          break;
        case 52:
          myTeknikByggnad.Beskrivning = attributeValue;
          break;
        case 53:
          myTeknikByggnad.Besktningsklass = attributeValue;
          break;
        case 54:
          myTeknikByggnad.Senast_Ändrad = attributeValue;
          break;
        case 55:
          myTeknikByggnad.Senast_Ändrad_Av = attributeValue;
          break;
        case 56:
          myTeknikByggnad.Notering = attributeValue;
          break;
        case 57:
          myTeknikByggnad.Nivå = attributeValue;
          break;
        case 58:
          myTeknikByggnad.Komponent = attributeValue;
          break;
        case 59:
          myTeknikByggnad.Placering = attributeValue;
          break;
        case 60:
          myTeknikByggnad.KomponentModell = attributeValue;
          break;
        case 61:
          myTeknikByggnad.Fabrikat = attributeValue;
          break;
        case 62:
          myTeknikByggnad.SerieNummer = attributeValue;
          break;
        case 63:
          myTeknikByggnad.Storlek = attributeValue;
          break;
        case 64:
          myTeknikByggnad.InstDatum = attributeValue;
          break;
      }
      return myTeknikByggnad;
    }

    public override Container ObjectStructure(List<BIS_GrundObjekt> bisList)
    {
      var formattedList = bisList;
      var container = new Container();
      var containerSoftypes = new List<SoftType>();

      //List of non-FT softypes
      var geografiskaPlaceringar = new List<GeografiskPlaceringsreferensInstances>();
      var specifikationer = new List<AnläggningsspecifikationInstances>();
      var produkter = new List<AnläggningsproduktInstances>();
      var funktionellaAnläggningar = new List<FunktionellAnläggningInstances>();
      var material = new List<MaterialkompositInstances>();
      var styckevaror = new List<StyckevaraInstances>();

      foreach (BIS_Teknikbyggnad bisTeknikbyggnad in formattedList)
      {
        var suffix = bisTeknikbyggnad.ObjektTypNummer + bisTeknikbyggnad.ObjektNummer + ExtraCounter;

        //Noterings fix
        bisTeknikbyggnad.Notering = string.IsNullOrEmpty(bisTeknikbyggnad.Notering)
          ? "Ingen Notering"
          : bisTeknikbyggnad.Notering;

        var funktionellTeknikbyggnadssystem = new FunktionellTeknikbyggnadssystem
        {
          name = "FunktionellTeknikbyggnadssystem",
          notering = bisTeknikbyggnad.Notering,
          versionId = _VersionId,
          företeelsetyp = new ClassificationReference_FunktionellAnläggning_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTFunktionellAnläggningReference
            {
              softType = "FTFunktionellAnläggning",
              instanceRef = "FunktionellTeknikbyggnadssystem"
            }
          },
          numericSet = new FunktionellTeknikbyggnadssystemNumericSet(),
          stringSet = new FunktionellTeknikbyggnadssystemStringSet
          {
            SystemID = new FunktionellTeknikbyggnadssystem_SystemID
            {
              generalProperty = new SystemID
              {
                softType = _SoftTypeProperty,
                instanceRef = "SystemID"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              Array = true,
              value = bisTeknikbyggnad.Beteckning
            }
          }
        };

        funktionellTeknikbyggnadssystem.id = funktionellTeknikbyggnadssystem.name + suffix;

        var teknikbyggnad = new Teknikbyggnad
        {
          name = "Teknikbyggnad",
          notering = bisTeknikbyggnad.Notering,
          versionId = _VersionId,
          företeelsetyp = new ClassificationReference_GeografiskPlaceringsreferens_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTGeografiskPlaceringsreferensReference
            {
              softType = "FTGeografiskPlaceringsreferens",
              instanceRef = "Teknikbyggnad"
            }
          }
        };
        teknikbyggnad.id = teknikbyggnad.name + suffix;

        var funktionellTeknikbyggnad = new FunktionellTeknikbyggnad
        {
          name = "FunktionellTeknikbyggnad",
          notering = bisTeknikbyggnad.Notering,
          versionId = _VersionId,
          numericSet = new FunktionellTeknikbyggnadNumericSet(),
          stringSet = new FunktionellTeknikbyggnadStringSet
          {
            typ = new FunktionellTeknikbyggnad_typ
            {
              Array = true,
              JSonMapToPropertyName = _JsonMapToValue,
              generalProperty = new typ
              {
                softType = _SoftTypeProperty,
                instanceRef = "typ"
              },
              value = bisTeknikbyggnad.Typ
            }
          },
          företeelsetyp = new ClassificationReference_FunktionellAnläggning_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTFunktionellAnläggningReference
            {
              softType = "FTFunktionellAnläggning",
              instanceRef = "FunktionellTeknikbyggnad"
            }
          }
        };
        funktionellTeknikbyggnad.id = funktionellTeknikbyggnad.name + suffix;

        var teknikbyggnadProdukt = new TeknikbyggnadProdukt
        {
          name = "TeknikbyggnadProdukt",
          notering = bisTeknikbyggnad.Notering,
          versionId = _VersionId,
          stringSet = new TeknikbyggnadProduktStringSet
          {
            typ = new TeknikbyggnadProdukt_typ
            {
              generalProperty = new typ
              {
                softType = _SoftTypeProperty,
                instanceRef = "typ"
              },
              Array = true,
              JSonMapToPropertyName = _JsonMapToValue,
              value = bisTeknikbyggnad.Typ
            }
          },
          numericSet = new TeknikbyggnadProduktNumericSet(),
          företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTAnläggningsproduktReference
            {
              softType = "FTAnläggningsprodukt",
              instanceRef = "TeknikbyggnadProdukt"
            }
          }
        };
        teknikbyggnadProdukt.id = teknikbyggnadProdukt.name + suffix;

        var fasadbeklädnadMaterial = new FasadbeklädnadMaterial
        {
          name = "FasadbeklädnadMaterial",
          notering = bisTeknikbyggnad.Notering,
          versionId = _VersionId,
          företeelsetyp = new ClassificationReference_Materialkomposit_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTMaterialkompositReference
            {
              softType = "FTMaterialkomposit",
              instanceRef = "FasadbeklädnadMaterial"
            }
          },
          stringSet = new FasadbeklädnadMaterialStringSet
          {
            typ = new FasadbeklädnadMaterial_typ
            {
              Array = true,
              generalProperty = new typ
              {
                softType = _SoftTypeProperty,
                instanceRef = "typ"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              value = bisTeknikbyggnad.Fasadutförande
            }
          },
          numericSet = new FasadbeklädnadMaterialNumericSet()
        };
        fasadbeklädnadMaterial.id = fasadbeklädnadMaterial.name + suffix;

        var funktionellÅskskyddsystem = new FunktionellÅskskyddssystem
        {
          name = "FunktionellÅskskyddssystem",
          notering = bisTeknikbyggnad.Notering,
          versionId = _VersionId,
          företeelsetyp = new ClassificationReference_FunktionellAnläggning_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTFunktionellAnläggningReference
            {
              softType = "FTFunktionellAnläggning",
              instanceRef = "FunktionellÅskskyddssystem"
            }
          },
          numericSet = new FunktionellÅskskyddssystemNumericSet(),
          stringSet = new FunktionellÅskskyddssystemStringSet
          {
            åskskyddsnivå = new FunktionellÅskskyddssystem_åskskyddsnivå
            {
              Array = true,
              generalProperty = new åskskyddsnivå
              {
                softType = _SoftTypeProperty,
                instanceRef = "åskskyddsnivå"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              value = bisTeknikbyggnad.Åskskydd
            }
          }
        };
        funktionellÅskskyddsystem.id = funktionellÅskskyddsystem.name + suffix;

        var funktionellElkraftförsörjning = new FunktionellElkraftförsörjning
        {
          name = "FunktionellElkraftförsörjning",
          notering = bisTeknikbyggnad.Notering,
          versionId = _VersionId,
          företeelsetyp = new ClassificationReference_FunktionellAnläggning_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTFunktionellAnläggningReference
            {
              softType = "FTFunktionellAnläggning",
              instanceRef = "FunktionellElkraftförsörjning"
            }
          },
          numericSet = new FunktionellElkraftförsörjningNumericSet(),
          stringSet = new FunktionellElkraftförsörjningStringSet
          {
            typ = new FunktionellElkraftförsörjning_typ
            {
              Array = true,
              JSonMapToPropertyName = _JsonMapToValue,
              generalProperty = new typ
              {
                softType = _SoftTypeProperty,
                instanceRef = "typ"
              },
              value = bisTeknikbyggnad.OrdinarieNät
            }
          }
        };
        funktionellElkraftförsörjning.id = funktionellElkraftförsörjning.name + suffix;

        var elkraftförsörjningSpecifikation = new ElkraftförsörjningSpecifikation
        {
          name = "ElkraftförsörjningSpecifikation",
          notering = bisTeknikbyggnad.Notering,
          versionId = _VersionId,
          företeelsetyp = new ClassificationReference_Anläggningsspecifikation_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTAnläggningsspecifikationReference
            {
              softType = "FTAnläggningsspecifikation",
              instanceRef = "ElkraftförsörjningSpecifikation"
            }
          },
          numericSet = new ElkraftförsörjningSpecifikationNumericSet(),
          stringSet = new ElkraftförsörjningSpecifikationStringSet
          {
            faser = new ElkraftförsörjningSpecifikation_faser
            {
              generalProperty = new faser
              {
                softType = _SoftTypeProperty,
                instanceRef = "faser"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              value = bisTeknikbyggnad.OrtsnätFaser
            },
            skyddstransformatorKapacitet = new ElkraftförsörjningSpecifikation_skyddstransformatorKapacitet
            {
              generalProperty = new skyddstransformatorKapacitet
              {
                softType = _SoftTypeProperty,
                instanceRef = "skyddstransformatorKapacitet"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              value = bisTeknikbyggnad.MellantrafoStorlek
            },
            säkring = new ElkraftförsörjningSpecifikation_säkring
            {
              generalProperty = new säkring
              {
                softType = _SoftTypeProperty,
                instanceRef = "säkring"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              value = bisTeknikbyggnad.OrtsnätSäkring
            }
          }
        };
        elkraftförsörjningSpecifikation.id = elkraftförsörjningSpecifikation.name + suffix;

        var elkraftförsörjningsProdukt = new ElkraftförsörjningProdukt
        {
          name = "ElkraftförsörjningProdukt",
          notering = bisTeknikbyggnad.Notering,
          versionId = _VersionId,
          företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTAnläggningsproduktReference
            {
              softType = "FTAnläggningsprodukt",
              instanceRef = "ElkraftförsörjningProdukt"
            }
          }
        };
        elkraftförsörjningsProdukt.id = elkraftförsörjningsProdukt.name + suffix;

        var elkraftförsörjningIndivid = new ElkraftförsörjningIndivid
        {
          name = "ElkraftförsörjningIndivid",
          notering = bisTeknikbyggnad.Notering,
          versionId = _VersionId,
          startSpecified = false,
          endSpecified = false,
          företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTStyckevaraReference
            {
              softType = "FTStyckevara",
              instanceRef = "ElkraftförsörjningIndivid"
            }
          },
          stringSet = new ElkraftförsörjningIndividStringSet
          {
            OrtsNätsavtal = new ElkraftförsörjningIndivid_OrtsNätsavtal
            {
              Array = true,
              generalProperty = new OrtsNätsavtal
              {
                softType = _SoftTypeProperty,
                instanceRef = "OrtsNätsavtal"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              value = bisTeknikbyggnad.OrtsnätIDNummer
            }
          },
          numericSet = new ElkraftförsörjningIndividNumericSet()
        };
        elkraftförsörjningIndivid.id = elkraftförsörjningIndivid.name + suffix;

        var funktionellReservekraft = new FunktionellReservkraft
        {
          name = "FunktionellReservkraft",
          notering = bisTeknikbyggnad.Notering,
          versionId = _VersionId,
          företeelsetyp = new ClassificationReference_FunktionellAnläggning_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTFunktionellAnläggningReference
            {
              softType = "FTFunktionellAnläggning",
              instanceRef = "FunktionellReservkraft"
            }
          },
          stringSet = new FunktionellReservkraftStringSet
          {
            typ = new FunktionellReservkraft_typ
            {
              Array = true,
              JSonMapToPropertyName = _JsonMapToValue,
              generalProperty = new typ
              {
                softType = _SoftTypeProperty,
                instanceRef = "typ"
              },
              value = bisTeknikbyggnad.RedundantNät
            }
          },
          numericSet = new FunktionellReservkraftNumericSet()
        };
        funktionellReservekraft.id = funktionellReservekraft.name + suffix;

        //TODO: Vart kommer drifttid ifrån
        var reservkraftSpecifikation = new ReservkraftSpecifikation
        {
          name = "ReservkraftSpecifikation",
          notering = bisTeknikbyggnad.Notering,
          versionId = _VersionId,
          företeelsetyp = new ClassificationReference_Anläggningsspecifikation_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTAnläggningsspecifikationReference
            {
              softType = "FTAnläggningsspecifikation",
              instanceRef = "ReservkraftSpecifikation"
            }
          },
          numericSet = new ReservkraftSpecifikationNumericSet
          {
            drifttid = new ReservkraftSpecifikation_drifttid
            {
              generalProperty = new drifttid
              {
                softType = "Property",
                instanceRef = "drifttid"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              Unit = new h
              {
                softType = _SoftTypeUnit,
                instanceRef = "h"
              },
              value = 0
            },
            kapacitet = new ReservkraftSpecifikation_kapacitet
            {
              generalProperty = new kapacitet
              {
                softType = _SoftTypeProperty,
                instanceRef = "kapacitet"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              Unit = new kVA
              {
                softType = _SoftTypeUnit,
                instanceRef = "kVA"
              },
              value = 0
            }
          },
          stringSet = new ReservkraftSpecifikationStringSet
          {
            faser = new ReservkraftSpecifikation_faser
            {
              generalProperty = new faser
              {
                softType = _SoftTypeProperty,
                instanceRef = "faser"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              value = bisTeknikbyggnad.HjälpkraftFaser
            },
            säkring = new ReservkraftSpecifikation_säkring
            {
              generalProperty = new säkring
              {
                softType = _SoftTypeProperty,
                instanceRef = "säkring"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              value = bisTeknikbyggnad.HjälpkraftSäkring
            }
          }
        };
        reservkraftSpecifikation.id = reservkraftSpecifikation.name + suffix;

        var reservelverkProdukt = new ReservelverkProdukt
        {
          name = "ReservelverkProdukt",
          notering = bisTeknikbyggnad.Notering,
          versionId = _VersionId,
          företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTAnläggningsproduktReference
            {
              softType = "FTAnläggningsprodukt",
              instanceRef = "ReservelverkProdukt"
            }
          },
          numericSet = new ReservelverkProduktNumericSet
          {
            kapacitet = new ReservelverkProdukt_kapacitet
            {
              generalProperty = new kapacitet
              {
                softType = _SoftTypeProperty,
                instanceRef = "kapacitet"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              Unit = new kVA
              {
                softType = _SoftTypeUnit,
                instanceRef = "kVA"
              },
              value = CreateLength(bisTeknikbyggnad.ReservelverkStorlek) 
            },
            tankvolym = new ReservelverkProdukt_tankvolym
            {
              generalProperty = new tankvolym
              {
                softType = _SoftTypeProperty,
                instanceRef = "tankvolym"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              Unit = new l
              {
                softType = _SoftTypeProperty,
                instanceRef = "l"
              },
              value = CreateLength(bisTeknikbyggnad.ReservelverkTankvolym) 
            }
          },
          stringSet = new ReservelverkProduktStringSet()
        };
        reservelverkProdukt.id = reservelverkProdukt.name + suffix;

        var batteriSpecifikation = new BatteriSpecifikation
        {
          name = "BatteriSpecifikation",
          notering = bisTeknikbyggnad.Notering,
          versionId = _VersionId,
          företeelsetyp = new ClassificationReference_Anläggningsspecifikation_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTAnläggningsspecifikationReference
            {
              softType = "FTAnläggningsspecifikation",
              instanceRef = "BatteriSpecifikation"
            }
          },
          numericSet = new BatteriSpecifikationNumericSet
          {
            kapacitet = new BatteriSpecifikation_kapacitet
            {
              generalProperty = new kapacitet
              {
                softType = _SoftTypeProperty,
                instanceRef = "kapacitet"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              Unit = new Ah
              {
                softType = _SoftTypeUnit,
                instanceRef = "Ah"
              },
              //Finns inget värde?
              value = 0
            },
            spänning = new BatteriSpecifikation_spänning
            {
              generalProperty = new spänning
              {
                softType = _SoftTypeProperty,
                instanceRef = "spänning"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              Unit = new V
              {
                softType = _SoftTypeUnit,
                instanceRef = "V"
              },
              //Finns inget värde
              value = 0
            }
          },
          stringSet = new BatteriSpecifikationStringSet()
        };
        batteriSpecifikation.id = batteriSpecifikation.name + suffix;

        var batteriProdukt = new BatteriProdukt
        {
          name = "BatteriProdukt",
          notering = bisTeknikbyggnad.Notering,
          versionId = _VersionId,
          företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTAnläggningsproduktReference
            {
              softType = "FTAnläggningsprodukt",
              instanceRef = "BatteriProdukt"
            }
          },
          numericSet = new BatteriProduktNumericSet
          {
            kapacitet = new BatteriProdukt_kapacitet
            {
              Array = true,
              JSonMapToPropertyName = _JsonMapToValue,
              generalProperty = new kapacitet
              {
                softType = _SoftTypeProperty,
                instanceRef = "kapacitet"
              },
              Unit = new Ah
              {
                softType = _SoftTypeUnit,
                instanceRef = "Ah"
              },
              value = 0
            }
          },
          stringSet = new BatteriProduktStringSet()
        };
        batteriProdukt.id = batteriProdukt.name + suffix;

        var batteriIndivid = new BatteriIndivid
        {
          name = "BatteriIndivid",
          versionId = _VersionId,
          notering = bisTeknikbyggnad.Notering,
          startSpecified = false,
          endSpecified = false,
          företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTStyckevaraReference
            {
              softType = "FTStyckevara",
              instanceRef = "BatteriIndivid"
            }
          }
        };
        batteriIndivid.id = batteriIndivid.name + suffix;

        var funktionellKlimatanläggning = new FunktionellKlimatanläggning
        {
          name = "FunktionellKlimatanläggning",
          versionId = _VersionId,
          notering = bisTeknikbyggnad.Notering,
          företeelsetyp = new ClassificationReference_FunktionellAnläggning_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTFunktionellAnläggningReference
            {
              softType = "FTFunktionellAnläggning",
              instanceRef = "FunktionellKlimatanläggning"
            }
          }
        };
        funktionellKlimatanläggning.id = funktionellKlimatanläggning.name + suffix;

        var klimatanläggningProdukt = new KlimatanläggningProdukt
        {
          name = "KlimatanläggningProdukt",
          versionId = _VersionId,
          notering = bisTeknikbyggnad.Notering,
          företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTAnläggningsproduktReference
            {
              softType = "FTAnläggningsprodukt",
              instanceRef = "KlimatanläggningProdukt"
            }
          },
          numericSet = new KlimatanläggningProduktNumericSet
          {
            effektförbrukning = new KlimatanläggningProdukt_effektförbrukning
            {
              Array = true,
              generalProperty = new effektförbrukning
              {
                softType = _SoftTypeProperty,
                instanceRef = "effektförbrukning"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              Unit = new kVA
              {
                softType = _SoftTypeProperty,
                instanceRef = "kVA"
              },
              value = 0
            }
          },
          stringSet = new KlimatanläggningProduktStringSet()
        };
        klimatanläggningProdukt.id = klimatanläggningProdukt.name + suffix;

        var klimatanläggningIndivid = new KlimatanläggningIndivid
        {
          name = "KlimatanläggningIndivid",
          startSpecified = false,
          endSpecified = false,
          versionId = _VersionId,
          notering = bisTeknikbyggnad.Notering,
          företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTStyckevaraReference
            {
              softType = "FTStyckevara",
              instanceRef = "KlimatanläggningIndivid"
            }
          }
        };
        klimatanläggningIndivid.id = klimatanläggningIndivid.name + suffix;

        var teknikbyggnadIndivid = new TeknikbyggnadIndivid
        {
          name = "TeknikbyggnadIndivid",
          startSpecified = false,
          endSpecified = false,
          versionId = _VersionId,
          notering = bisTeknikbyggnad.Notering,
          företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTStyckevaraReference
            {
              softType = "FTStyckevara",
              instanceRef = "TeknikbyggnadIndivid"
            }
          }
        };
        teknikbyggnadIndivid.id = teknikbyggnadIndivid.name + suffix;

        var reservelverkIndivid = new ReservelverkIndivid
        {
          name = "ReservelverkIndivid",
          startSpecified = false,
          endSpecified = false,
          versionId = _VersionId,
          notering = bisTeknikbyggnad.Notering,
          företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTStyckevaraReference
            {
              softType = "FTStyckevara",
              instanceRef = "ReservelverkIndivid"
            }
          }
        };
        reservelverkIndivid.id = reservelverkIndivid.name + suffix;

        var funktionellUps = new FunktionellUPS
        {
          name = "FunktionellUPS",
          versionId = _VersionId,
          notering = bisTeknikbyggnad.Notering,
          företeelsetyp = new ClassificationReference_FunktionellAnläggning_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTFunktionellAnläggningReference
            {
              softType = "FTFunktionellAnläggning",
              instanceRef = "FunktionellUPS"
            }
          }
        };
        funktionellUps.id = funktionellUps.name + suffix;

        var upsSpecifikation = new UPSSpecifikation
        {
          name = "UPSSpecifikation",
          versionId = _VersionId,
          notering = bisTeknikbyggnad.Notering,
          företeelsetyp = new ClassificationReference_Anläggningsspecifikation_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTAnläggningsspecifikationReference
            {
              softType = "FTAnläggningsspecifikation",
              instanceRef = "UPSSpecifikation"
            }
          },
          numericSet = new UPSSpecifikationNumericSet
          {
            minKapacitet = new UPSSpecifikation_minKapacitet
            {
              generalProperty = new minKapacitet
              {
                softType = _SoftTypeProperty,
                instanceRef = "minKapacitet"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              Unit = new kVA
              {
                softType = _SoftTypeUnit,
                instanceRef = "kVA"
              },
              value = 0
            },
            spänning = new UPSSpecifikation_spänning
            {
              generalProperty = new spänning
              {
                softType = _SoftTypeProperty,
                instanceRef = "spänning"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              Unit = new V
              {
                softType = _SoftTypeUnit,
                instanceRef = "V"
              },
              value = 0
            }
          },
          stringSet = new UPSSpecifikationStringSet()
        };
        upsSpecifikation.id = upsSpecifikation.name + suffix;

        var upsProdukt = new UPSProdukt
        {
          name = "UPSProdukt",
          versionId = _VersionId,
          notering = bisTeknikbyggnad.Notering,
          företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTAnläggningsproduktReference
            {
              softType = "FTAnläggningsprodukt",
              instanceRef = "UPSProdukt"
            }
          },
          numericSet = new UPSProduktNumericSet
          {
            kapacitet = new UPSProdukt_kapacitet
            {
              Array = true,
              generalProperty = new kapacitet
              {
                softType = _SoftTypeProperty,
                instanceRef = "kapacitet"
              },
              JSonMapToPropertyName = _JsonMapToValue,
              Unit = new kVA
              {
                softType = _SoftTypeUnit,
                instanceRef = "kVA"
              },
              value = 0
            }
          },
          stringSet = new UPSProduktStringSet()
        };
        upsProdukt.id = upsProdukt.name + suffix;

        var upsIndivid = new UPSIndivid
        {
          name = "UPSIndivid",
          versionId = _VersionId,
          notering = bisTeknikbyggnad.Notering,
          startSpecified = false,
          endSpecified = false,
          företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTStyckevaraReference
            {
              softType = "FTStyckevara",
              instanceRef = "UPSIndivid"
            }
          }
        };
        upsIndivid.id = upsIndivid.name + suffix;

        // ***---***---***---*** ENTRY INSTANCES ***---***---***---***

        var funktionellTeknikbyggnadssystemEntry = new FunktionellAnläggningEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "FunktionellTeknikbyggnadssystem" + suffix,
          data = funktionellTeknikbyggnadssystem
        };
        funktionellaAnläggningar.Add(funktionellTeknikbyggnadssystemEntry);
        var teknikbyggnadEntry = new GeografiskPlaceringsreferensEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "Teknikbyggnad" + suffix,
          data = teknikbyggnad
        };
        geografiskaPlaceringar.Add(teknikbyggnadEntry);

        var funktionellTeknigbyggnadEntry = new FunktionellAnläggningEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "FunktionellTeknikbyggnad" + suffix,
          data = funktionellTeknikbyggnad
        };
        funktionellaAnläggningar.Add(funktionellTeknigbyggnadEntry);

        var teknikbyggnadProduktEntry = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "TeknikbyggnadProdukt" + suffix,
          data = teknikbyggnadProdukt
        };
        produkter.Add(teknikbyggnadProduktEntry);

        var fasadBeklädnadMaterialEntry = new MaterialkompositEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "FasadbeklädnadMaterial" + suffix,
          data = fasadbeklädnadMaterial
        };
        material.Add(fasadBeklädnadMaterialEntry);

        var funktionellÅskskyddssystemEntry = new FunktionellAnläggningEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "FunktionellÅskskyddssystem" + suffix,
          data = funktionellÅskskyddsystem
        };
        funktionellaAnläggningar.Add(funktionellÅskskyddssystemEntry);

        var funktionellElkraftförsörjningEntry = new FunktionellAnläggningEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "FunktionellElkraftförsörjning" + suffix,
          data = funktionellElkraftförsörjning
        };
        funktionellaAnläggningar.Add(funktionellElkraftförsörjningEntry);

        var elkraftförsörjningSpecifikationEntry = new AnläggningsspecifikationEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "ElkraftförsörjningSpecifikation" + suffix,
          data = elkraftförsörjningSpecifikation
        };
        specifikationer.Add(elkraftförsörjningSpecifikationEntry);

        var elkraftförsörjningsPriduktEntry = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "ElkraftförsörjningProdukt" + suffix,
          data = elkraftförsörjningsProdukt
        };
        produkter.Add(elkraftförsörjningsPriduktEntry);

        var elkraftförsörjningIndividEntry = new StyckevaraEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "ElkraftförsörjningsIndivid" + suffix,
          data = elkraftförsörjningIndivid
        };
        styckevaror.Add(elkraftförsörjningIndividEntry);

        var funktionellReservkraftEntry = new FunktionellAnläggningEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "FunktionellReservkraft" + suffix,
          data = funktionellReservekraft
        };
        funktionellaAnläggningar.Add(funktionellReservkraftEntry);

        var reservkraftSpecifikationEntry = new AnläggningsspecifikationEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "ReservekraftSpecifikation" + suffix,
          data = reservkraftSpecifikation
        };
        specifikationer.Add(reservkraftSpecifikationEntry);

        var reservelverkProduktEntry = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "ReservelverkProdukt" + suffix,
          data = reservelverkProdukt
        };
        produkter.Add(reservelverkProduktEntry);

        var batteriSpecifikationEntry = new AnläggningsspecifikationEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "BatteriSpecifikation" + suffix,
          data = batteriSpecifikation
        };
        specifikationer.Add(batteriSpecifikationEntry);

        var batteriProduktEntry = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "BatteriSpecifikation" + suffix,
          data = batteriProdukt
        };
        produkter.Add(batteriProduktEntry);

        var batteriIndividEntry = new StyckevaraEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "BatteriIndivid" + suffix,
          data = batteriIndivid
        };
        styckevaror.Add(batteriIndividEntry);

        var funktionellKlimatanläggningEntry = new FunktionellAnläggningEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "FunktionellKlimatanläggning" + suffix,
          data = funktionellKlimatanläggning
        };
        funktionellaAnläggningar.Add(funktionellKlimatanläggningEntry);

        var klimatanläggningProduktEntry = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "KlimatanläggningProdukt" + suffix,
          data = klimatanläggningProdukt
        };
        produkter.Add(klimatanläggningProduktEntry);

        var klimatanläggningIndividEntry = new StyckevaraEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "KlimatanläggningIndivid" + suffix,
          data = klimatanläggningIndivid
        };
        styckevaror.Add(klimatanläggningIndividEntry);

        var teknikbyggnadIndividEntry = new StyckevaraEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "TeknikbyggnadIndivid" + suffix,
          data = teknikbyggnadIndivid
        };
        styckevaror.Add(teknikbyggnadIndividEntry);

        var reservelverkIndividEntry = new StyckevaraEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "ReservelverkIndivid" + suffix,
          data = reservelverkIndivid
        };
        styckevaror.Add(reservelverkIndividEntry);

        var funktionellUPSEntry = new FunktionellAnläggningEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "FunktionellUPS" + suffix,
          data = funktionellUps
        };
        funktionellaAnläggningar.Add(funktionellUPSEntry);

        var upsSpecifikationEntry = new AnläggningsspecifikationEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "UPSSpecifikation" + suffix,
          data = upsSpecifikation
        };
        specifikationer.Add(upsSpecifikationEntry);

        var upsProduktEntry = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "UPSProdukt",
          data = upsProdukt
        };
        produkter.Add(upsProduktEntry);

        var upsIndividEntry = new StyckevaraEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "UPSIndivid" + suffix,
          data = upsIndivid
        };
        styckevaror.Add(upsIndividEntry);

        ExtraCounter++;
      }

      // ***---***---***---*** SOFTTYPES ***---***---***---***

      var geografiskPlaceringsReferensSoftType = new SoftType_GeografiskPlaceringsreferens
      {
        Array = true,
        id = "GeografiskPlaceringsreferens",
        instances = geografiskaPlaceringar.ToArray()
      };
      containerSoftypes.Add(geografiskPlaceringsReferensSoftType);

      var anläggningsSpecifikationSoftType = new SoftType_Anläggningsspecifikation
      {
        Array = true,
        id = "Anläggningsspecifikation",
        instances = specifikationer.ToArray()
      };
      containerSoftypes.Add(anläggningsSpecifikationSoftType);

      var anläggningsProduktSoftType = new SoftType_Anläggningsprodukt
      {
        Array = true,
        id = "Anläggningsprodukt",
        instances = produkter.ToArray()
      };
      containerSoftypes.Add(anläggningsProduktSoftType);

      var funktionellAnläggningSoftType = new SoftType_FunktionellAnläggning
      {
        Array = true,
        id = "FunktionellAnläggning",
        instances = funktionellaAnläggningar.ToArray()
      };
      containerSoftypes.Add(funktionellAnläggningSoftType);

      var materialkompositSoftType = new SoftType_Materialkomposit
      {
        Array = true,
        id = "Materialkomposit",
        instances = material.ToArray()
      };
      containerSoftypes.Add(materialkompositSoftType);

      var styckevaraSoftType = new SoftType_Styckevara
      {
        Array = true,
        id = "Styckevara",
        instances = styckevaror.ToArray()

      };
      containerSoftypes.Add(styckevaraSoftType);

      //MORE SOFTTYPES!
      containerSoftypes.AddRange(CreateSupplementarySoftypes());
      //Not started
      containerSoftypes.AddRange(CreateFTKeyReferenceSoftTypes());

      container.softTypes = containerSoftypes.ToArray();
      return container;
    }

    public override List<SoftType> CreateFTKeyReferenceSoftTypes()
    {
      var softtypeList = new List<SoftType>();

      var ftgpr = new FTGeografiskPlaceringsreferensEntrydefaultIn
      {
        Array = true,
        id = "Teknikbyggnad",
        inputSchemaRef = _InputSchemaRef,
        data = new FTGeografiskPlaceringsreferensdefaultIn
        {
          id = "Teknikbyggnad",
          name = FeatureTypeName
        }
      };
      var FTGeografiskPlaceringsreferensInstances = new List<FTGeografiskPlaceringsreferensInstances> { ftgpr };
      var FTGPRSoftType = new SoftType_FTGeografiskPlaceringsreferens
      {
        Array = true,
        id = "SoftType_GeografiskPlaceringsreferens",
        instances = FTGeografiskPlaceringsreferensInstances.ToArray()
      };
      softtypeList.Add(FTGPRSoftType);

      //FTAnläggningsProdukt
      var teknikbyggnadProdukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "TeknikbyggnadProdukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "TeknikbyggnadProdukt",
          name = FeatureTypeName
        }
      };

      var elkraftförsörjningProdukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "ElkraftförsörjningProdukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "ElkraftförsörjningProdukt",
          name = FeatureTypeName
        }
      };

      var reservelverkProdukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "ReservelverkProdukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "ReservelverkProdukt",
          name = FeatureTypeName
        }
      };

      var batteriProdukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "BatteriProdukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "BatteriProdukt",
          name = FeatureTypeName
        }
      };

      var klimatanläggningProdukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "KlimatanläggningProdukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "KlimatanläggningProdukt",
          name = FeatureTypeName
        }
      };

      var uPSProdukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "UPSProdukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "UPSProdukt",
          name = FeatureTypeName
        }
      };


      var FTAnläggningsproduktInstances = new List<FTAnläggningsproduktInstances>
      {
        teknikbyggnadProdukt, elkraftförsörjningProdukt, reservelverkProdukt, batteriProdukt,
        klimatanläggningProdukt, uPSProdukt
      };
      var FTAnläggningsProduktSoftType = new SoftType_FTAnläggningsprodukt
      {
        Array = true,
        id = "FTAnläggningsprodukt",
        instances = FTAnläggningsproduktInstances.ToArray()
      };
      softtypeList.Add(FTAnläggningsProduktSoftType);
      //FTAnläggningsProdukt END

      //FTFunktionellAnläggning
      var funktionellTeknikbyggnadssystem = new FTFunktionellAnläggningEntrydefaultIn
      {
        Array = true,
        id = "FunktionellTeknikbyggnadssystem",
        inputSchemaRef = _InputSchemaRef,
        data = new FTFunktionellAnläggningdefaultIn
        {
          id = "FunktionellTeknikbyggnadssystem",
          name = FeatureTypeName
        }
      };

      var funktionellTeknikbyggnad = new FTFunktionellAnläggningEntrydefaultIn
      {
        Array = true,
        id = "FunktionellTeknikbyggnad",
        inputSchemaRef = _InputSchemaRef,
        data = new FTFunktionellAnläggningdefaultIn
        {
          id = "FunktionellTeknikbyggnad",
          name = FeatureTypeName
        }
      };

      var funktionellÅskskyddssystem = new FTFunktionellAnläggningEntrydefaultIn
      {
        Array = true,
        id = "FunktionellÅskskyddssystem",
        inputSchemaRef = _InputSchemaRef,
        data = new FTFunktionellAnläggningdefaultIn
        {
          id = "FunktionellÅskskyddssystem",
          name = FeatureTypeName
        }
      };

      var funktionellElkraftförsörjning = new FTFunktionellAnläggningEntrydefaultIn
      {
        Array = true,
        id = "FunktionellElkraftförsörjning",
        inputSchemaRef = _InputSchemaRef,
        data = new FTFunktionellAnläggningdefaultIn
        {
          id = "FunktionellElkraftförsörjning",
          name = FeatureTypeName
        }
      };

      var funktionellReservkraft = new FTFunktionellAnläggningEntrydefaultIn
      {
        Array = true,
        id = "FunktionellReservkraft",
        inputSchemaRef = _InputSchemaRef,
        data = new FTFunktionellAnläggningdefaultIn
        {
          id = "FunktionellReservkraft",
          name = FeatureTypeName
        }
      };

      var funktionellKlimatanläggning = new FTFunktionellAnläggningEntrydefaultIn
      {
        Array = true,
        id = "FunktionellKlimatanläggning",
        inputSchemaRef = _InputSchemaRef,
        data = new FTFunktionellAnläggningdefaultIn
        {
          id = "FunktionellKlimatanläggning",
          name = FeatureTypeName
        }
      };

      var funktionellUPS = new FTFunktionellAnläggningEntrydefaultIn
      {
        Array = true,
        id = "FunktionellUPS",
        inputSchemaRef = _InputSchemaRef,
        data = new FTFunktionellAnläggningdefaultIn
        {
          id = "FunktionellUPS",
          name = FeatureTypeName
        }
      };

      var FTFunktionellaAnläggningarInstances = new List<FTFunktionellAnläggningInstances>
      {
        funktionellTeknikbyggnadssystem, funktionellTeknikbyggnad, funktionellÅskskyddssystem,
        funktionellElkraftförsörjning, funktionellReservkraft, funktionellKlimatanläggning,
        funktionellUPS
      };
      var FTFunktionellAnläggningSoftType = new SoftType_FTFunktionellAnläggning
      {
        Array = true,
        id = "FTFunktionellAnläggning",
        instances = FTFunktionellaAnläggningarInstances.ToArray()
      };
      softtypeList.Add(FTFunktionellAnläggningSoftType);
      //FTFunktionellAnläggning END

      //FTAnläggningsspecifikation
      var elkraftförsörjningSpecifikation = new FTAnläggningsspecifikationEntrydefaultIn
      {
        Array = true,
        id = "ElkraftförsörjningSpecifikation",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsspecifikationdefaultIn
        {
          id = "ElkraftförsörjningSpecifikation",
          name = FeatureTypeName
        }
      };

      var reservkraftSpecifikation = new FTAnläggningsspecifikationEntrydefaultIn
      {
        Array = true,
        id = "ReservkraftSpecifikation",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsspecifikationdefaultIn
        {
          id = "ReservkraftSpecifikation",
          name = FeatureTypeName
        }
      };

      var batteriSpecifikation = new FTAnläggningsspecifikationEntrydefaultIn
      {
        Array = true,
        id = "BatteriSpecifikation",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsspecifikationdefaultIn
        {
          id = "BatteriSpecifikation",
          name = FeatureTypeName
        }
      };

      var uPSSpecifikation = new FTAnläggningsspecifikationEntrydefaultIn
      {
        Array = true,
        id = "UPSSpecifikation",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsspecifikationdefaultIn
        {
          id = "UPSSpecifikation",
          name = FeatureTypeName
        }
      };

      var FTAnläggningsspecifikationInstances = new List<FTAnläggningsspecifikationInstances>
      {
        elkraftförsörjningSpecifikation, reservkraftSpecifikation, batteriSpecifikation, uPSSpecifikation
      };
      var FTAnläggningsspecifikationSoftType = new SoftType_FTAnläggningsspecifikation
      {
        Array = true,
        id = "FTAnläggningsspecifikation",
        instances = FTAnläggningsspecifikationInstances.ToArray()
      };
      softtypeList.Add(FTAnläggningsspecifikationSoftType);
      //FTAnläggningsspecifikation END

      //FTStyckevara
      var elkraftförsörjningIndivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "ElkraftförsörjningIndivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "ElkraftförsörjningIndivid",
          name = FeatureTypeName
        }
      };

      var batteriIndivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "BatteriIndivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "BatteriIndivid",
          name = FeatureTypeName
        }
      };

      var klimatanläggningIndivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "KlimatanläggningIndivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "KlimatanläggningIndivid",
          name = FeatureTypeName
        }
      };

      var teknikbyggnadIndivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "TeknikbyggnadIndivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "TeknikbyggnadIndivid",
          name = FeatureTypeName
        }
      };

      var reservelverkIndivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "ReservelverkIndivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "ReservelverkIndivid",
          name = FeatureTypeName
        }
      };

      var uPSIndivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "UPSIndivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "UPSIndivid",
          name = FeatureTypeName
        }
      };

      var FTStyckevaraInstances = new List<FTStyckevaraInstances>
      {
        elkraftförsörjningIndivid, batteriIndivid, klimatanläggningIndivid, teknikbyggnadIndivid,
        reservelverkIndivid, uPSIndivid
      };
      var FTStyckevaraSoftType = new SoftType_FTStyckevara
      {
        Array = true,
        id = "FTStyckevara",
        instances = FTStyckevaraInstances.ToArray()
      };
      softtypeList.Add(FTStyckevaraSoftType);
      //FTStyckevara END

      //FTMaterial
      var fasadbeklädnadMaterial = new FTMaterialkompositEntrydefaultIn
      {
        Array = true,
        id = "FasadbeklädnadMaterial",
        inputSchemaRef = _InputSchemaRef,
        data = new FTMaterialkompositdefaultIn
        {
          id = "FasadbeklädnadMaterial",
          name = FeatureTypeName
        }
      };
      var FTMaterialkompositer = new List<FTMaterialkompositInstances>
      { fasadbeklädnadMaterial };
      var FTMaterialkompositSoftType = new SoftType_FTMaterialkomposit
      {
        Array = true,
        id = "FTFunktionellAnläggning",
        instances = FTMaterialkompositer.ToArray()
      };
      softtypeList.Add(FTMaterialkompositSoftType);
      //FTMaterial END
      return softtypeList;
    }
  }
}
