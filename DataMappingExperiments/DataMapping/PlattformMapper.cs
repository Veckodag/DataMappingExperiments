using System;
using System.Collections.Generic;
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
    public override string Name => "Plattform";

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
          myPlattform.BesiktningsKlass = attributeValue;
          break;
        case 24:
          myPlattform.Senast_Ändrad = attributeValue;
          break;
        case 25:
          myPlattform.Senast_Ändrad_Av = attributeValue;
          break;
        case 26:
          myPlattform.Notering = attributeValue;
          break;
      }
      return myPlattform;
    }

    public override void ObjectStructure(List<BIS_GrundObjekt> bisList)
    {
      foreach (BIS_Plattform bisPlattform in bisList)
      {
        Plattform plattform = new Plattform
        {
          id = Guid.NewGuid().ToString(),
          notering = bisPlattform.Notering,
          name = "Name",
          stringSet = new PlattformStringSet
          {
            Väderskydd = new Plattform_Väderskydd(),
            Beläggning = new Plattform_Beläggning(),
            Brunnolock = new Plattform_Brunnolock(),
            Fotsteg = new Plattform_Fotsteg(),
            Höjd = new Plattform_Höjd(),
            Höjd_beskr = new Plattform_Höjd_beskr(),
            Kmtal = new Plattform_Kmtal(),
            Kmtalti = new Plattform_Kmtalti(),
            Skyddsräcken = new Plattform_Skyddsräcken(),
            Skylt = new Plattform_Skylt(),
            Plattformsutrustning = new Plattform_Plattformsutrustning(),
            Plattformskantmtrl = new Plattform_Plattformskantmtrl(),
            Skyddszonochledstråk = new Plattform_Skyddszonochledstråk()
          },
          numericSet = new PlattformNumericSet
          {
            Breddm = new Plattform_Breddm(),
            Längdm = new Plattform_Längdm()
          }
        };
      }
    }
  }
}
