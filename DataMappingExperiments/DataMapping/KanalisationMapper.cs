using System;
using System.Collections.Generic;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments.DataMapping
{
  class KanalisationMapper : Mapper
  {
    public KanalisationMapper()
    {
      MapperType = MapperType.Kanalisation;
      FeatureTypeName = "Läst från fil BIS_Kanalisation - datadefinition infomod 2p0.xlsm";
    }

    public sealed override MapperType MapperType { get; set; }

    public override BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt bisObject)
    {
      var myKanalisation = (BIS_Kanalisation)bisObject;

      switch (index)
      {
        case 0:
          myKanalisation.ObjektTypNummer = attributeValue;
          break;
        case 1:
          myKanalisation.ObjektNummer = attributeValue;
          break;
        case 30:
          myKanalisation.Typ = attributeValue;
          break;
        case 31:
          myKanalisation.MaterialKanalisation = attributeValue;
          break;
        case 32:
          myKanalisation.MaterialLock = attributeValue;
          break;
        case 33:
          myKanalisation.DiameterBrunnRör = attributeValue;
          break;
        case 34:
          myKanalisation.BreddRänna = attributeValue;
          break;
        case 35:
          myKanalisation.AntalRör = attributeValue;
          break;
        case 37:
          myKanalisation.Senast_Ändrad = attributeValue;
          break;
        case 38:
          myKanalisation.Senast_Ändrad_Av = attributeValue;
          break;
        case 39:
          myKanalisation.Notering = attributeValue;
          break;
      }

      return myKanalisation;
    }

    public override Container ObjectStructure(List<BIS_GrundObjekt> bisList)
    {
      Container container = new Container();
      var containerSofttypes = new List<SoftType>();

      var anläggningsprodukter = new List<AnläggningsproduktInstances>();
      var styckevaror = new List<StyckevaraInstances>();

      foreach (BIS_Kanalisation bisKanalisation in bisList)
      {
        var suffix = bisKanalisation.ObjektTypNummer + bisKanalisation.ObjektNummer + ExtraCounter;

        //Noterings fix
        bisKanalisation.Notering = string.IsNullOrEmpty(bisKanalisation.Notering)
          ? "Ingen Notering"
          : bisKanalisation.Notering;

        var kanalisationsprodukt = CreateKanalisationProdukt(bisKanalisation, suffix);
        var kanalisationindivid = CreateKanalisationIndivid(bisKanalisation, suffix);
        
        var kanalisationproduktEntry = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "kanalisationsprodukt" + suffix,
          data = kanalisationsprodukt
        };
        anläggningsprodukter.Add(kanalisationproduktEntry);

        var kanalisationindividEntry = new StyckevaraEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "kanalisationindivid" + suffix,
          data = kanalisationindivid
        };
        styckevaror.Add(kanalisationindividEntry);

        ExtraCounter++;
      }

      var anläggningsproduktsofttype = new SoftType_Anläggningsprodukt
      {
        Array = true,
        id = "Anläggningsprodukt",
        instances = anläggningsprodukter.ToArray()
      };

      var styckevarasofttype = new SoftType_Styckevara
      {
        Array = true,
        id = "Styckevara",
        instances = styckevaror.ToArray()
      };
      containerSofttypes.Add(anläggningsproduktsofttype);
      containerSofttypes.Add(styckevarasofttype);
      containerSofttypes.AddRange(CreateSupplementarySoftypes());
      containerSofttypes.AddRange(CreateFTKeyReferenceSoftTypes());

      container.softTypes = containerSofttypes.ToArray();

      return container;

    }

    public override List<SoftType> CreateFTKeyReferenceSoftTypes()
    {
      var softtypeList = new List<SoftType>();

      //FTAnläggningsProdukt
      var kanalisationprodukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "Kanalisationprodukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "Kanalisationprodukt",
          name = "Läst från fil BIS_Kanalisation - datadefinition infomod 2p0.xlsm BIS_Plattform - datadefinition infomod 2p0.xlsm"
        }
      };

      var FTAnläggningsproduktInstances = new List<FTAnläggningsproduktInstances>{ kanalisationprodukt };
      var FTAnläggningsProduktSoftType = new SoftType_FTAnläggningsprodukt
      {
        Array = true,
        id = "FTAnläggningsprodukt",
        instances = FTAnläggningsproduktInstances.ToArray()
      };
      softtypeList.Add(FTAnläggningsProduktSoftType);
      //FTAnläggningsProdukt END

      var kanalisationindivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "Kanalisationindivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "Kanalisationindivid",
          name = "Läst från fil BIS_Kanalisation - datadefinition infomod 2p0.xlsm BIS_Plattform - datadefinition infomod 2p0.xlsm"
        }
      };

      var FTStyckevaraInstances = new List<FTStyckevaraInstances>{ kanalisationindivid };
      var FTStyckevaraSoftType = new SoftType_FTStyckevara
      {
        Array = true,
        id = "FTStyckevara",
        instances = FTStyckevaraInstances.ToArray()
      };
      softtypeList.Add(FTStyckevaraSoftType);

      return softtypeList;
    }

    private Kanalisationprodukt CreateKanalisationProdukt(BIS_Kanalisation p, string suffix)
    {
      Kanalisationprodukt o = new Kanalisationprodukt
      {
        name = "Kanalisationprodukt",
        versionId = _VersionId,
        notering = p.Notering,
        stringSet = new KanalisationproduktStringSet
        {
          materialKanalisation = new Kanalisationprodukt_materialKanalisation
          {
            value = p.MaterialKanalisation,
            generalProperty = new materialKanalisation
            {
              instanceRef = "materialKanalisation",
              softType = _SoftTypeProperty
            },
            JSonMapToPropertyName = _JsonMapToValue
          },
          materialLock = new Kanalisationprodukt_materialLock
          {
            value = p.MaterialLock,
            generalProperty = new materialLock
            {
              instanceRef = "materialLock",
              softType = _SoftTypeProperty
            },
            JSonMapToPropertyName = _JsonMapToValue
          },
          typ = new Kanalisationprodukt_typ
          {
            value = p.Typ,
            generalProperty = new typ
            {
              instanceRef = "typ",
              softType = _SoftTypeProperty
            },
            JSonMapToPropertyName = _JsonMapToValue
          }
        },
        företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
        {
          @class = new FTAnläggningsproduktReference
          {
            instanceRef = "Kanalisationprodukt",
            softType = "FTAnläggningsproduktReference"
          },
          startSpecified = false,
          endSpecified = false
        }
      };
      o.id = o.name + suffix;

      return o;
    }

    private Kanalisationindivid CreateKanalisationIndivid(BIS_Kanalisation p, string suffix)
    {
      Kanalisationindivid o = new Kanalisationindivid
      {
        name = "Kanalisationindivid",
        versionId = _VersionId,
        notering = p.Notering,
        företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
        {
          @class = new FTStyckevaraReference
          {
            instanceRef = "Kanalisationindivid",
            softType = "FTStyckevaraReference"
          },
          startSpecified = false,
          endSpecified = false
        }
      };

      o.id = o.name + suffix;
      return o;
    }

  }
}
