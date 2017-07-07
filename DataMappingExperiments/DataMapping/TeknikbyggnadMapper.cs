using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments.DataMapping
{
  public class TeknikbyggnadMapper : Mapper
  {
    public TeknikbyggnadMapper()
    {
      MapperType = MapperType.Teknikbyggnad;
    }
    public sealed override MapperType MapperType { get; set; }
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
        case 29:
          myTeknikByggnad.Modell = attributeValue;
          break;
        case 30:
          myTeknikByggnad.Beteckning = attributeValue;
          break;
        case 31:
          myTeknikByggnad.ByggnadsÅr = attributeValue;
          break;
        case 32:
          myTeknikByggnad.Typ = attributeValue;
          break;
        case 33:
          myTeknikByggnad.Fasadutförande = attributeValue;
          break;
        case 34:
          myTeknikByggnad.Åskskydd = attributeValue;
          break;
        case 35:
          myTeknikByggnad.OrdinarieNät = attributeValue;
          break;
        case 36:
          myTeknikByggnad.RedundantNät = attributeValue;
          break;
        case 37:
          myTeknikByggnad.HjälpkraftFaser = attributeValue;
          break;
        case 38:
          myTeknikByggnad.HjälpkraftSäkring = attributeValue;
          break;
        case 39:
          myTeknikByggnad.OrtsnätFaser = attributeValue;
          break;
        case 40:
          myTeknikByggnad.OrtsnätSäkring = attributeValue;
          break;
        case 41:
          myTeknikByggnad.OrtsnätIDNummer = attributeValue;
          break;
        case 42:
          myTeknikByggnad.OrtsnätÄgare = attributeValue;
          break;
        case 43:
          myTeknikByggnad.MellantrafoStorlek = attributeValue;
          break;
        case 44:
          myTeknikByggnad.Sidoavstånd = attributeValue;
          break;
        case 45:
          myTeknikByggnad.Northing = attributeValue;
          break;
        case 46:
          myTeknikByggnad.Easting = attributeValue;
          break;
        case 47:
          myTeknikByggnad.ReservelverkUtförande = attributeValue;
          break;
        case 48:
          myTeknikByggnad.ReservelverkStorlek = attributeValue;
          break;
        case 49:
          myTeknikByggnad.ReservelverkInstÅr = attributeValue;
          break;
        case 50:
          myTeknikByggnad.ReservelverkTankvolym = attributeValue;
          break;
        case 51:
          myTeknikByggnad.ReservelverkBatterimodell = attributeValue;
          break;
        case 52:
          myTeknikByggnad.ReserveelverkBatteriInDatum = attributeValue;
          break;
          //53 is not used
        case 54:
          myTeknikByggnad.Besktningsklass = attributeValue;
          break;
        case 55:
          myTeknikByggnad.Senast_Ändrad = attributeValue;
          break;
        case 56:
          myTeknikByggnad.Senast_Ändrad_Av = attributeValue;
          break;
        case 57:
          myTeknikByggnad.Notering = attributeValue;
          break;
          //58 is not used
        case 59:
          myTeknikByggnad.Komponent = attributeValue;
          break;
        case 60:
          myTeknikByggnad.Placering = attributeValue;
          break;
        case 61:
          myTeknikByggnad.ModellTyp = attributeValue;
          break;
        case 62:
          myTeknikByggnad.Fabrikat = attributeValue;
          break;
        case 63:
          myTeknikByggnad.SerieNummer = attributeValue;
          break;

      }

      return myTeknikByggnad;
    }

    public override Container ObjectStructure(List<BIS_GrundObjekt> bisList)
    {
      throw new NotImplementedException();
    }
  }
}
