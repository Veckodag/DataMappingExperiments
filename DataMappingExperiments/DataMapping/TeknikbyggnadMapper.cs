using System.Collections.Generic;
using System.Linq;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments.DataMapping
{
  public class TeknikbyggnadMapper : Mapper
  {
    public TeknikbyggnadMapper()
    {
      MapperType = MapperType.Teknikbyggnad;
      ExtraCounter = 1;
    }
    public sealed override MapperType MapperType { get; set; }
    private bool _IsComponent = false;
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
      //Does not use the SquashList method
      var formattedList = bisList;
      var container = new Container();
      var containerSoftypes = new List<SoftType>();

      foreach (BIS_Teknikbyggnad bisTeknikbyggnad in formattedList)
      {
        var funktionellTeknikbyggnadssystem = new FunktionellTeknikbyggnadssystem
        {
          name = "FunktionellTeknikbyggnadssystem",
          notering = bisTeknikbyggnad.Notering,
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

        funktionellTeknikbyggnadssystem.id = funktionellTeknikbyggnadssystem.name + bisTeknikbyggnad.ObjektTypNummer + bisTeknikbyggnad.ObjektNummer;
        var teknikbyggnad = new Teknikbyggnad
        {
          name = "Teknikbyggnad",
          notering = bisTeknikbyggnad.Notering,
          versionId = "001"
        };
        teknikbyggnad.id = teknikbyggnad.name + bisTeknikbyggnad.ObjektTypNummer + bisTeknikbyggnad.ObjektNummer;

        var funktionellTeknikbyggnad = new FunktionellTeknikbyggnad
        {
          name = "FunktionellTeknikbyggnad",
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
          notering = bisTeknikbyggnad.Notering
        };
        funktionellTeknikbyggnad.id = funktionellTeknikbyggnad.name + bisTeknikbyggnad.ObjektTypNummer +
                                      bisTeknikbyggnad.ObjektNummer;

        
        var teknikbyggnadProdukt = new TeknikbyggnadProdukt();
        var fasadbeklädnadMaterial = new FasadbeklädnadMaterial();
        var funktionellÅskskyddsystem = new FunktionellÅskskyddssystem();
        var funktionellElkraftförsörjning = new FunktionellElkraftförsörjning();
        var elkraftförsörjningSpecifikation = new ElkraftförsörjningSpecifikation();
        var elkraftförsörjningsProdukt = new ElkraftförsörjningProdukt();
        var elkraftförsörjningIndivid = new ElkraftförsörjningIndivid();
        var funktionellReservekraft = new FunktionellReservkraft();
        var reservkraftSpecifikation = new ReservkraftSpecifikation();
        var reservelverkProdukt = new ReservelverkProdukt();
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

    public override IEnumerable<BIS_GrundObjekt> SquashTheList(List<BIS_GrundObjekt> bisList)
    {
      var myList = new List<BIS_Teknikbyggnad>();

      foreach (var objekt in bisList)
        myList.Add(objekt as BIS_Teknikbyggnad);

      myList = myList.GroupBy(objektDetalj => objektDetalj.ObjektNummer).Select(values => values.FirstOrDefault()).ToList();

      return myList;
    }
  }
}
