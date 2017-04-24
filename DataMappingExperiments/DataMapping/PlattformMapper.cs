using System;
using System.Collections.Generic;
using System.Xml.Serialization;
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
      Container container = new Container();
      container.softTypes = new SoftType[4];
      
      foreach (BIS_Plattform bisPlattform in bisList)
      {
        Plattform plattform = new Plattform
        {
          id = Guid.NewGuid().ToString(),
          notering = bisPlattform.Notering,
          name = "Name?",
          arbetsnamn = "arbetsnamn?",
          versionId = "versionsId",
          stringSet = new PlattformStringSet
          {
            Väderskydd = SkapaPlattformVäderskydd(bisPlattform, new Plattform_Väderskydd()),
            Beläggning = SkapaPlattformBeläggning(bisPlattform, new Plattform_Beläggning()),
            Brunnolock = SkapaPlattformBrunnOLock(bisPlattform, new Plattform_Brunnolock()),
            Fotsteg = SkapaPlattformFotsteg(bisPlattform, new Plattform_Fotsteg()),
            Höjd = SkapaPlattformHöjd(bisPlattform, new Plattform_Höjd()),
            Höjd_beskr = SkapaPlattformHöjdBeskrivning(bisPlattform, new Plattform_Höjd_beskr()),
            Skyddsräcken = SkapaPlattfomrSkyddsräcken(bisPlattform, new Plattform_Skyddsräcken()),
            Skylt = SkapaPlattformSkylt(bisPlattform, new Plattform_Skylt()),
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

    private Plattform_Skylt SkapaPlattformSkylt(BIS_Plattform bisPlattform, Plattform_Skylt plattformSkylt)
    {
      throw new NotImplementedException();
    }

    private Plattform_Skyddsräcken SkapaPlattfomrSkyddsräcken(BIS_Plattform bisPlattform, Plattform_Skyddsräcken plattformSkyddsräcken)
    {
      throw new NotImplementedException();
    }

    private Plattform_Höjd_beskr SkapaPlattformHöjdBeskrivning(BIS_Plattform bisPlattform, Plattform_Höjd_beskr plattformHöjdBeskr)
    {
      throw new NotImplementedException();
    }

    private Plattform_Höjd SkapaPlattformHöjd(BIS_Plattform bisPlattform, Plattform_Höjd plattformHöjd)
    {
      Höjd höjd = new Höjd
      {
        instanceRef = "Höjd",
        softType = "Property"
      };

      plattformHöjd.generalProperty = höjd;
      plattformHöjd.value = bisPlattform.Höjd;
      plattformHöjd.JSonMapToPropertyName = "value";

      return plattformHöjd;
    }

    private Plattform_Fotsteg SkapaPlattformFotsteg(BIS_Plattform bisPlattform, Plattform_Fotsteg plattformFotsteg)
    {
      Fotsteg fotsteg = new Fotsteg
      {
        instanceRef = "Fotsteg",
        softType = "Property"
      };

      plattformFotsteg.generalProperty = fotsteg;
      plattformFotsteg.value = bisPlattform.Fotsteg;
      plattformFotsteg.JSonMapToPropertyName = "value";

      return plattformFotsteg;
    }

    private Plattform_Brunnolock SkapaPlattformBrunnOLock(BIS_Plattform bisPlattform, Plattform_Brunnolock plattformBrunnolock)
    {
      //Kanalisation
      Brunnolock brunnolock = new Brunnolock
      {
        instanceRef = "Brunnolock",
        softType = "Property"
      };

      plattformBrunnolock.generalProperty = brunnolock;
      plattformBrunnolock.value = bisPlattform.Brunn_Och_Lock;
      plattformBrunnolock.JSonMapToPropertyName = "value";

      return plattformBrunnolock;
    }

    private Plattform_Beläggning SkapaPlattformBeläggning(BIS_Plattform bisPlattform, Plattform_Beläggning plattformBeläggning)
    {
      Beläggning beläggning = new Beläggning
      {
        instanceRef = "Beläggning",
        softType = "Property"
      };

      plattformBeläggning.generalProperty = beläggning;
      plattformBeläggning.value = bisPlattform.Beläggning;
      plattformBeläggning.JSonMapToPropertyName = "value";

      return plattformBeläggning;
    }

    private Plattform_Väderskydd SkapaPlattformVäderskydd(BIS_Plattform bisPlattform, Plattform_Väderskydd plattformVäderskydd)
    {
      Väderskydd väderskydd = new Väderskydd
      {
        instanceRef = "Väderskydd",
        softType = "Property"
      };

      plattformVäderskydd.generalProperty = väderskydd;
      plattformVäderskydd.value = bisPlattform.Väderskydd;
      //Not sure, should be a XML attribute
      plattformVäderskydd.JSonMapToPropertyName = "value";

      return plattformVäderskydd;
    }

    public class XmlAttributeDataMapper<T>
    {
      [XmlAttribute]
      public T Value { get; set; }
    }
  }
}
