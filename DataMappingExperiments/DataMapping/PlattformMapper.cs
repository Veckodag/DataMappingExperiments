using System;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments.DataMapping
{
  public class PlattformMapper : Mapper
  {
    public PlattformMapper()
    {
      MapperType = MapperType.Plattform;
    }

    public override MapperType MapperType { get; set; }

    public override void HelloFromThisMappingclass()
    {
      Console.WriteLine("I'am a plattform mapper!");
    }
    public override string MapXmlAttribute(int index, string attributeValue)
    {
      return attributeValue;
    }

    public override BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt plattform)
    {
      var myPlattform = (BIS_Plattform) plattform;
      switch (index)
      {
        case 11:
          myPlattform.Höjd = attributeValue;
          break;
        case 12:
          myPlattform.Längd = int.Parse(attributeValue);
          break;
        case 13:
          myPlattform.Bredd = attributeValue;
          break;
        case 14:
          myPlattform.Plattformskant_mtrl = attributeValue;
          break;
        case 15:
          myPlattform.Beläggning = attributeValue;
          break;
        case 16:
          myPlattform.Skyddszon_Och_Ledstråk = attributeValue;
          break;
        case 17:
          myPlattform.Väderskydd = attributeValue;
          break;
        case 18:
          myPlattform.Skylt = attributeValue;
          break;
        case 19:
          myPlattform.Fotsteg = attributeValue;
          break;
        case 20:
          myPlattform.Brunn_Och_Lock = attributeValue;
          break;
        case 21:
          myPlattform.Skyddsräcken = attributeValue;
          break;
        case 22:
          myPlattform.PlattformsUtrustning = attributeValue;
          break;
        case 23:
          myPlattform.PlattformsUtrustning = attributeValue;
          break;
        case 24:
          myPlattform.BesiktningsKlass = attributeValue;
          break;
        case 25:
          myPlattform.Senast_Ändrad = attributeValue;
          break;
        case 26:
          myPlattform.Senast_Ändrad_Av = attributeValue;
          break;
        case 27:
          myPlattform.Notering = attributeValue;
          break;
      }
      return myPlattform;
    }
  }
}
