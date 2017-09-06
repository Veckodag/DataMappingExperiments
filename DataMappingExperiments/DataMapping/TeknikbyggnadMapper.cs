using System.Collections.Generic;
using System.Linq;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DataMappingExperiments.DataMapping
{
  public class TeknikbyggnadMapper : Mapper
  {
    public TeknikbyggnadMapper()
    {
      MapperType = MapperType.Teknikbyggnad;
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

      foreach (BIS_Teknikbyggnad bisTeknikbyggnad in formattedList)
      {
        var suffix = bisTeknikbyggnad.ObjektTypNummer + bisTeknikbyggnad.ObjektNummer + ExtraCounter;
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
              instanceRef = Program.SelectedDataContainer.Name
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
              instanceRef = Program.SelectedDataContainer.Name
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
              instanceRef = Program.SelectedDataContainer.Name
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
              instanceRef = Program.SelectedDataContainer.Name
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
              instanceRef = Program.SelectedDataContainer.Name
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
              instanceRef = Program.SelectedDataContainer.Name
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
              instanceRef = Program.SelectedDataContainer.Name
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
              instanceRef = Program.SelectedDataContainer.Name
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
              instanceRef = Program.SelectedDataContainer.Name
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
              instanceRef = Program.SelectedDataContainer.Name
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
              instanceRef = Program.SelectedDataContainer.Name
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
              instanceRef = Program.SelectedDataContainer.Name
            }
          },
          //numericSet = new ReservkraftSpecifikationNumericSet
          //{
          //  drifttid = new ReservkraftSpecifikation_drifttid
          //  {
          //    generalProperty = new drifttid
          //    {
          //      softType = "Property",
          //      instanceRef = "drifttid"
          //    },
          //    JSonMapToPropertyName = _JsonMapToValue,
          //  },
          //  kapacitet = new ReservkraftSpecifikation_kapacitet()
          //},
          stringSet = new ReservkraftSpecifikationStringSet
          {
            faser = new ReservkraftSpecifikation_faser()
            {
              generalProperty = new faser()
            },
            säkring = new ReservkraftSpecifikation_säkring()
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
              instanceRef = Program.SelectedDataContainer.Name
            }
          }
        };
        var reservelverkIndivid = new ReservelverkIndivid();
        var batteriSpecifikation = new BatteriSpecifikation();
        var batteriProdukt = new BatteriProdukt();
        var batteriIndivid = new BatteriIndivid();
        var funktionellKlimatanläggning = new FunktionellKlimatanläggning();
        var klimatanläggningProdukt = new KlimatanläggningProdukt();
        var klimatanläggningIndivid = new KlimatanläggningIndivid();
        var funktionellups = new FunktionellUPS();
        var upsSpecifikation = new UPSSpecifikation();
        var upsProdukt = new UPSProdukt();
        var upsIndivid = new UPSIndivid();

        //Not sure if used
        var teknikbyggnadIndivid = new TeknikbyggnadIndivid();

        ExtraCounter++;
      }
      container.softTypes = containerSoftypes.ToArray();
      return container;
    }

    public override List<SoftType> CreateFTKeyReferenceSoftTypes()
    {
      throw new System.NotImplementedException();
    }
  }
}
