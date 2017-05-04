﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using DataMappingExperiments.AndaObjekt;
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
          myPlattform.Längd = decimal.Parse(attributeValue, CultureInfo.InvariantCulture);
          break;
        case 13:
          myPlattform.Bredd = decimal.Parse(attributeValue, CultureInfo.InvariantCulture);
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
      var containerSoftTypes = new List<SoftType>();
      //var plattformar = new List<GeografiskPlaceringsreferensdefaultIn>();
         
      //foreach (BIS_Plattform bisPlattform in bisList)
      //{
      //  var geografiskSofttype = new SoftType_GeografiskPlaceringsreferens();
      //  var ftGeografiskSofttype = new SoftType_FTGeografiskPlaceringsreferens();
      //  var plattformInstance = new GeografiskPlaceringsreferensEntrydefaultIn();
      //  var specifikationInstance = new GeografiskPlaceringsreferensdefaultIn();
      //  var företeelseTyp = new ClassificationReference_GeografiskPlaceringsreferens_företeelsetyp();
      //  var typ = new FTGeografiskPlaceringsreferensReference();

      //  företeelseTyp.@class = typ;
      //  specifikationInstance.anläggningsprodukt = new BreakdownElementRealization_GeografiskPlaceringsreferens_anläggningsprodukt[4];

      //  plattformInstance.data = specifikationInstance;
      //  ANDA_Plattform plattform = new ANDA_Plattform
      //  {
      //    id = Guid.NewGuid().ToString(),
      //    notering = bisPlattform.Notering,
      //    name = "Name?",
      //    arbetsnamn = "arbetsnamn?",
      //    versionId = "versionsId",
      //    stringSet = new ANDA_PlattformStringset
      //    {
      //      Väderskydd = SkapaPlattformVäderskydd(bisPlattform, new Plattform_Väderskydd()),
      //      Beläggning = SkapaPlattformBeläggning(bisPlattform, new Plattform_Beläggning()),
      //      Brunnolock = SkapaPlattformBrunnOLock(bisPlattform, new Plattform_Brunnolock()),
      //      Fotsteg = SkapaPlattformFotsteg(bisPlattform, new Plattform_Fotsteg()),
      //      Höjd = SkapaPlattformHöjd(bisPlattform, new Plattform_Höjd()),
      //      Skyddsräcken = SkapaPlattformSkyddsräcken(bisPlattform, new Plattform_Skyddsräcken()),
      //      Skylt = SkapaPlattformSkylt(bisPlattform, new Plattform_Skylt()),
      //      Plattformsutrustning = SkapaPlattformsUtrustning(bisPlattform, new Plattform_Plattformsutrustning()),
      //      Plattformskantmtrl = SkapaPlattformsKantMaterial(bisPlattform, new Plattform_Plattformskantmtrl()),
      //      Skyddszonochledstråk = SkapaPlattformSkyddszonOchLedstråk(bisPlattform, new Plattform_Skyddszonochledstråk())
      //    },
      //    numericSet = new ANDA_PlattformNumericSet
      //    {
      //      Breddm = SkapaPlattformBredd(bisPlattform, new Plattform_Breddm()),
      //      Längdm = SkapaPlattformLängd(bisPlattform, new Plattform_Längdm())
      //    }
      //  };
      //  //add to list!
      //  specifikationInstance = plattform;
      //  plattformar.Add(specifikationInstance);
      //}

      //Test data!
      // ANDA plattform funkar inte än, solve it!
      var myTestPlattform = new GeografiskPlaceringsreferensEntrydefaultIn
      {
        data = new Plattform
        {
          id = "007",
          notering = "Test",
          versionId = "1001",
          name = "Test-Plattform"
        }
      };

      var testGeografiskSofttype = new SoftType_GeografiskPlaceringsreferens();
      var testInstanceList = new List<GeografiskPlaceringsreferensInstances> {myTestPlattform};
      testGeografiskSofttype.instances = testInstanceList.ToArray();
      containerSoftTypes.Add(testGeografiskSofttype);
      container.softTypes = containerSoftTypes.ToArray();
      
      XmlSerializer serializer = new XmlSerializer(typeof(Container));
      TextWriter tw = new StreamWriter(@"c:\temp\plattform.xml");
      serializer.Serialize(tw, container);
    }

    private Plattform_Längdm SkapaPlattformLängd(BIS_Plattform bisPlattform, Plattform_Längdm plattformLängdm)
    {
      Längdm1 längd = new Längdm1
      {
        instanceRef = "Längdm",
        softType = "Property"
      };
      
      plattformLängdm.generalProperty = längd;
      plattformLängdm.value = bisPlattform.Längd;
      plattformLängdm.JSonMapToPropertyName = "value";

      return plattformLängdm;
    }

    private Plattform_Breddm SkapaPlattformBredd(BIS_Plattform bisPlattform, Plattform_Breddm plattformBreddm)
    {
      Breddm bredd = new Breddm
      {
        instanceRef = "Breddm",
        softType = "Property"
      };
      
      plattformBreddm.generalProperty = bredd;
      plattformBreddm.value = bisPlattform.Bredd;
      plattformBreddm.JSonMapToPropertyName = "value";
      //EMPTY UNIT?
      
      return plattformBreddm;
    }

    private Plattform_Skyddszonochledstråk SkapaPlattformSkyddszonOchLedstråk(BIS_Plattform bisPlattform, Plattform_Skyddszonochledstråk plattformSkyddszonochledstråk)
    {
      Skyddszonochledstråk skyddszonochledstråk = new Skyddszonochledstråk
      {
        instanceRef = "Skyddszonochledstråk",
        softType = "Property"
      };

      if (bisPlattform.Skyddszon_Och_Ledstråk != "?")
        bisPlattform.Skyddszon_Och_Ledstråk = "Ja";

      plattformSkyddszonochledstråk.generalProperty = skyddszonochledstråk;
      plattformSkyddszonochledstråk.value = bisPlattform.Skyddszon_Och_Ledstråk;
      plattformSkyddszonochledstråk.JSonMapToPropertyName = "value";

      return plattformSkyddszonochledstråk;
    }

    private Plattform_Plattformskantmtrl SkapaPlattformsKantMaterial(BIS_Plattform bisPlattform, Plattform_Plattformskantmtrl plattformPlattformskantmtrl)
    {
      Plattformskantmtrl plattformskantmtrl = new Plattformskantmtrl
      {
        instanceRef = "Plattformskantmtrl",
        softType = "Property"
      };

      plattformPlattformskantmtrl.generalProperty = plattformskantmtrl;
      plattformPlattformskantmtrl.value = bisPlattform.Plattformskant_mtrl;
      plattformPlattformskantmtrl.JSonMapToPropertyName = "value";

      return plattformPlattformskantmtrl;
    }

    private Plattform_Plattformsutrustning SkapaPlattformsUtrustning(BIS_Plattform bisPlattform, Plattform_Plattformsutrustning plattformPlattformsutrustning)
    {
      Plattformsutrustning plattformsutrustning = new Plattformsutrustning
      {
        instanceRef = "Plattformsutrustning",
        softType = "Property"
      };

      if (bisPlattform.PlattformsUtrustning == "?")
        bisPlattform.PlattformsUtrustning = "Okänt";

      plattformPlattformsutrustning.generalProperty = plattformsutrustning;
      plattformPlattformsutrustning.value = bisPlattform.PlattformsUtrustning;
      plattformPlattformsutrustning.JSonMapToPropertyName = "value";

      return plattformPlattformsutrustning;

    }

    private Plattform_Skylt SkapaPlattformSkylt(BIS_Plattform bisPlattform, Plattform_Skylt plattformSkylt)
    {
      Skylt skylt = new Skylt
      {
        instanceRef = "Skylt",
        softType = "Property"
      };

      //Skylt objekt

      plattformSkylt.generalProperty = skylt;
      plattformSkylt.value = bisPlattform.Skylt;
      plattformSkylt.JSonMapToPropertyName = "value";

      return plattformSkylt;
    }

    private Plattform_Skyddsräcken SkapaPlattformSkyddsräcken(BIS_Plattform bisPlattform, Plattform_Skyddsräcken plattformSkyddsräcken)
    {
      Skyddsräcken skyddsräcken = new Skyddsräcken
      {
        instanceRef = "Skyddsräcken",
        softType = "Property"
      };

      if (bisPlattform.Skyddsräcken == "?")
        bisPlattform.Skyddsräcken = "Okänt";

      plattformSkyddsräcken.generalProperty = skyddsräcken;
      plattformSkyddsräcken.value = bisPlattform.Skyddsräcken;
      plattformSkyddsräcken.JSonMapToPropertyName = "value";

      return plattformSkyddsräcken;

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

      if (bisPlattform.Fotsteg == "?")
        bisPlattform.Fotsteg = "Okänt";

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

      if (bisPlattform.Brunn_Och_Lock == "?")
        bisPlattform.Brunn_Och_Lock = "Okänt";

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

      // Typ av väderskydd: Skydd || Tak

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
