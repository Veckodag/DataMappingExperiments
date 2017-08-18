using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments.DataMapping
{
  public class SpårspärrMapper : Mapper
  {
    public SpårspärrMapper()
    {
      MapperType = MapperType.Spårspärr;
    }
    public sealed override MapperType MapperType { get; set; }
    public override BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt bisObject)
    {
      var mySpårspärr = (BIS_SpårSpärr) bisObject;

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
          mySpårspärr.ModellTyp = attributeValue;
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
      throw new NotImplementedException();
    }

    public override IEnumerable<BIS_GrundObjekt> SquashTheList(List<BIS_GrundObjekt> bisList)
    {
      //TODO: Concatenate
      throw new NotImplementedException();
    }
  }
}
