using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments.DataMapping
{
  public class PlattformMapper : Mapper
  {
    public PlattformMapper()
    {
      MapperType = MapperType.Plattform;
      FeatureTypeName = "Läst från fil BIS_Plattform - datadefinition infomod 2p0.xlsm";
    }
    public sealed override MapperType MapperType { get; set; }

    public override BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt bisObjekt)
    {
      var myPlattform = (BIS_Plattform)bisObjekt;
      //IT IS ZERO BASED
      switch (index)
      {
        case 0:
          myPlattform.ObjektTypNummer = attributeValue;
          break;
        case 1:
          myPlattform.ObjektNummer = attributeValue;
          break;
        case 17:
          myPlattform.Kmtal = attributeValue;
          break;
        case 20:
          myPlattform.Kmtalti = attributeValue;
          break;
        case 28:
          myPlattform.Höjd = attributeValue;
          break;
        case 29:
          myPlattform.Längd = decimal.Parse(attributeValue, CultureInfo.InvariantCulture);
          break;
        case 30:
          myPlattform.Bredd = decimal.Parse(attributeValue, CultureInfo.InvariantCulture);
          break;
        case 31:
          myPlattform.Plattformskant_mtrl = attributeValue;
          break;
        case 32:
          myPlattform.Beläggning = attributeValue;
          break;
        case 33:
          myPlattform.Skyddszon_Och_Ledstråk = attributeValue;
          break;
        case 34:
          myPlattform.Väderskydd = attributeValue;
          break;
        case 35:
          myPlattform.Skylt = attributeValue;
          break;
        case 36:
          myPlattform.Fotsteg = attributeValue;
          break;
        case 37:
          myPlattform.Brunn_Och_Lock = attributeValue;
          break;
        case 38:
          myPlattform.Skyddsräcken = attributeValue;
          break;
        case 39:
          myPlattform.PlattformsUtrustning = attributeValue;
          break;
        case 40:
          myPlattform.BesiktningsKlass = attributeValue;
          break;
        case 41:
          myPlattform.Senast_Ändrad = attributeValue;
          break;
        case 42:
          myPlattform.Senast_Ändrad_Av = attributeValue;
          break;
        case 43:
          myPlattform.Notering = attributeValue;
          break;
      }
      return myPlattform;
    }
    public override Container ObjectStructure(List<BIS_GrundObjekt> bisList)
    {
      var formattedBisList = bisList;
      Container container = new Container();
      //All softypes must be aggregated before they are added to the container
      var containerSoftTypes = new List<SoftType>();

      //Sort this mess out at some point
      var plattformsProdukter = new List<AnläggningsproduktInstances>();
      var plattformsFunktioner = new List<FunktionellAnläggningInstances>();
      var plattformVäderskydd = new List<AnläggningsproduktInstances>();
      var plattformStyckevaror = new List<StyckevaraInstances>();
      var plattformSkylt = new List<AnläggningsproduktInstances>();
      var plattformSkyltIndivid = new List<StyckevaraInstances>();
      var plattformKanalisation = new List<AnläggningsproduktInstances>();
      var plattformKanalisationIndivid = new List<StyckevaraInstances>();
      var plattformIndivider = new List<StyckevaraInstances>();
      //Does all the real mapping against ANDA resources
      foreach (BIS_Plattform bisPlattform in formattedBisList)
      {
        var suffix = bisPlattform.ObjektTypNummer + bisPlattform.ObjektNummer + ExtraCounter;

        //Noterings fix
        bisPlattform.Notering = string.IsNullOrEmpty(bisPlattform.Notering)
          ? "Ingen Notering"
          : bisPlattform.Notering;

        var plattformsProduktInstans = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "PlattformProduktAnläggningsprodukt" + suffix
        };

        var plattformprodukt = new Plattformprodukt
        {
          notering = bisPlattform.Notering,
          name = "Plattformprodukt",
          versionId = _VersionId,
          stringSet = new PlattformproduktStringSet
          {
            PlattformBeläggning = SkapaPlattformBeläggning(bisPlattform, new Plattformprodukt_PlattformBeläggning()),
            plattformskantMaterial = SkapaPlattformKantMaterial(bisPlattform, new Plattformprodukt_plattformskantMaterial()),
            nominellHöjd = new Plattformprodukt_nominellHöjd
            {
              generalProperty = new nominellHöjd
              {
                instanceRef = "nominellHöjd",
                softType = _SoftTypeProperty
              },
              value = bisPlattform.Höjd,
              JSonMapToPropertyName = _JsonMapToValue
            }
          },
          numericSet = new PlattformproduktNumericSet
          {
            verkligHöjd = SkapaVerkligHöjd(bisPlattform, new Plattformprodukt_verkligHöjd()),
            längd = new Plattformprodukt_längd
            {
              generalProperty = new längd
              {
                instanceRef = "längd",
                softType = _SoftTypeProperty
              },
              value = bisPlattform.Längd,
              JSonMapToPropertyName = _JsonMapToValue,
              Unit = new m
              {
                instanceRef = "m",
                softType = "Unit"
              }
            },
            bredd = new Plattformprodukt_bredd
            {
              generalProperty = new bredd
              {
                instanceRef = "bredd",
                softType = _SoftTypeProperty
              },
              value = bisPlattform.Bredd,
              JSonMapToPropertyName = _JsonMapToValue,
              Unit = new m
              {
                instanceRef = "m",
                softType = "Unit"
              }
            }
          },
          företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTAnläggningsproduktReference
            {
              softType = "FTAnläggningsprodukt",
              instanceRef = "Plattformprodukt"
            }
          }
        };
        plattformprodukt.id = plattformprodukt.name + suffix;
        plattformsProduktInstans.data = plattformprodukt;
        plattformsProdukter.Add(plattformsProduktInstans);

        var plattformsFunktionInstans = new FunktionellAnläggningEntrydefaultIn
        {
          Array = true,
          inputSchemaRef = _InputSchemaRef,
          id = "PlattformFunktionFunktionell" + suffix
        };

        var plattformFunktion = new Plattformfunktion
        {
          notering = bisPlattform.Notering,
          name = "PlattformFunktion",
          versionId = _VersionId,
          stringSet = new PlattformfunktionStringSet
          {
            harFotsteg = SkapaPlattformFotsteg(bisPlattform, new Plattformfunktion_harFotsteg()),
            harPlattformsutrustning = SkapaPlattformUtrustning(bisPlattform, new Plattformfunktion_harPlattformsutrustning()),
            harSkyddsräcken = SkapaPlattformSkyddsräcken(bisPlattform, new Plattformfunktion_harSkyddsräcken())
          },
          numericSet = new PlattformfunktionNumericSet(),
          företeelsetyp = new ClassificationReference_FunktionellAnläggning_företeelsetyp
          {
            @class = new FTFunktionellAnläggningReference
            {
              softType = "FTFunktionellAnläggning",
              instanceRef = "Plattformfunktion"
            }
          }
        };
        plattformFunktion = SkyddzonOchLedstråk(bisPlattform, plattformFunktion, new Plattformfunktion_harLedstråk(), new Plattformfunktion_harSkyddszon());

        plattformFunktion.id = plattformFunktion.name + suffix;
        plattformsFunktionInstans.data = plattformFunktion;
        plattformsFunktioner.Add(plattformsFunktionInstans);

        //Skydd eller Tak
        var plattformAnläggningsProduktInstans = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          id = "PlattformVäderskyddOchSkärmtakAnläggningsprodukt" + suffix,
          inputSchemaRef = _InputSchemaRef
        };

        var plattformStyckevaraInstans = new StyckevaraEntrydefaultIn
        {
          Array = true,
          id = "PlattformVäderskyddOchSkärmtakStyckevara" + suffix,
          inputSchemaRef = _InputSchemaRef
        };

        var väderskyddProdukt = new Väderskyddprodukt
        {
          name = "Väderskyddprodukt",
          notering = bisPlattform.Notering,
          versionId = _VersionId,
          företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTAnläggningsproduktReference
            {
              softType = "FTAnläggningsprodukt",
              instanceRef = "Väderskyddprodukt"
            }
          }
        };
        väderskyddProdukt.id = väderskyddProdukt.name + suffix;

        var väderskyddIndivid = new Väderskyddindivid
        {
          startSpecified = false,
          endSpecified = false,
          name = "Väderskyddindivid",
          notering = bisPlattform.Notering,
          versionId = _VersionId,
          företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTStyckevaraReference
            {
              softType = "FTStyckevara",
              instanceRef = "Väderskyddindivid"
            }
          }
        };
        väderskyddIndivid.id = väderskyddIndivid.name + suffix;

        var skärmtakProdukt = new Skärmtakprodukt
        {
          name = "Skärmtakprodukt",
          notering = bisPlattform.Notering,
          versionId = _VersionId,
          företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTAnläggningsproduktReference
            {
              softType = "FTAnläggningsprodukt",
              instanceRef = "Skärmtakprodukt"
            }
          }
        };
        skärmtakProdukt.id = skärmtakProdukt.name + suffix;

        var skärmtakIndivid = new Skärmtakindivid
        {
          startSpecified = false,
          endSpecified = false,
          name = "Skärmtakindivid",
          notering = bisPlattform.Notering,
          versionId = _VersionId,
          företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTStyckevaraReference
            {
              softType = "FTStyckevara",
              instanceRef = "Skärmtakindivid"
            }
          }
        };
        skärmtakIndivid.id = skärmtakIndivid.name + suffix;

        switch (bisPlattform.Väderskydd)
        {
          case "Skydd":
            plattformAnläggningsProduktInstans.data = väderskyddProdukt;
            plattformStyckevaraInstans.data = väderskyddIndivid;
            break;
          case "Tak":
            plattformAnläggningsProduktInstans.data = skärmtakProdukt;
            plattformStyckevaraInstans.data = skärmtakIndivid;
            break;
        }
        plattformVäderskydd.Add(plattformAnläggningsProduktInstans);
        plattformStyckevaror.Add(plattformStyckevaraInstans);
        //End Väderskydd

        //Skylt
        var skyltProdukt = new Fast_skyltprodukt
        {
          name = "Fast_skyltprodukt",
          notering = bisPlattform.Notering,
          versionId = _VersionId,
          företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTAnläggningsproduktReference
            {
              softType = "FTAnläggningsprodukt",
              instanceRef = "Fast_skyltprodukt"
            }
          }
        };
        skyltProdukt.id = skyltProdukt.name + suffix;

        var skyltIndivid = new Fast_skyltindivid
        {
          startSpecified = false,
          endSpecified = false,
          name = "Fast_skyltindivid",
          notering = bisPlattform.Notering,
          versionId = _VersionId,
          företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTStyckevaraReference
            {
              softType = "FTStyckevara",
              instanceRef = "Fast_skyltindivid"
            }
          }
        };
        skyltIndivid.id = skyltIndivid.name + suffix;

        var plattformSkyltAnläggningsProdukt = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          id = "PlattformSkyltAnläggningsprodukt" + suffix,
          inputSchemaRef = _InputSchemaRef,
          data = skyltProdukt
        };
        var plattformSkyltStyckevara = new StyckevaraEntrydefaultIn
        {
          Array = true,
          id = "PlattformSkyltStyckevara" + suffix,
          inputSchemaRef = _InputSchemaRef,
          data = skyltIndivid
        };
        if (bisPlattform.Skylt != "?")
        {
          plattformSkylt.Add(plattformSkyltAnläggningsProdukt);
          plattformSkyltIndivid.Add(plattformSkyltStyckevara);
        }

        //Kanalisation (BrunOLock)
        var kanalisationsProdukt = new Kanalisationprodukt
        {
          name = "Kanalisationprodukt",
          notering = bisPlattform.Notering,
          versionId = _VersionId,
          stringSet = new KanalisationproduktStringSet(),
          numericSet = new KanalisationproduktNumericSet(),
          företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTAnläggningsproduktReference
            {
              softType = "FTAnläggningsprodukt",
              instanceRef = "Kanalisationprodukt"
            }
          }
        };
        kanalisationsProdukt.id = kanalisationsProdukt.name + suffix;
        var kanalisationsIndivid = new Kanalisationindivid
        {
          name = "Kanalisationindivid",
          notering = bisPlattform.Notering,
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
              instanceRef = "Kanalisationindivid"
            }
          }
        };
        kanalisationsIndivid.id = kanalisationsIndivid.name + suffix;

        var plattformKanalisationProdukt = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          id = "PlattformKanalisationAnläggningsprodukt" + suffix,
          inputSchemaRef = _InputSchemaRef,
          data = kanalisationsProdukt
        };
        var plattformKanalisationStyckevara = new StyckevaraEntrydefaultIn
        {
          Array = true,
          id = "PlattformKanalisationStyckevara" + suffix,
          inputSchemaRef = _InputSchemaRef,
          data = kanalisationsIndivid
        };
        if (bisPlattform.Brunn_Och_Lock != "?")
        {
          plattformKanalisation.Add(plattformKanalisationProdukt);
          plattformKanalisationIndivid.Add(plattformKanalisationStyckevara);
        }

        //PlattformIndivid
        var plattformindivid = new Plattformindivid
        {
          startSpecified = false,
          endSpecified = false,
          name = "Plattformindivid",
          notering = bisPlattform.Notering,
          versionId = _VersionId,
          företeelsetyp = new ClassificationReference_Styckevara_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTStyckevaraReference
            {
              softType = "FTStyckevara",
              instanceRef = "Plattformindivid"
            }
          }
        };
        plattformindivid.id = plattformindivid.name + suffix;

        var plattformIndividStyckevara = new StyckevaraEntrydefaultIn
        {
          Array = true,
          id = "PlattformIndividStyckevara" + suffix,
          inputSchemaRef = _InputSchemaRef,
          data = plattformindivid
        };
        plattformIndivider.Add(plattformIndividStyckevara);

        ExtraCounter++;
      }
      var anläggningsProduktSoftType = new SoftType_Anläggningsprodukt
      {
        Array = true,
        id = "Anläggningsprodukt",
        instances = plattformsProdukter.ToArray()
      };
      var funktionellAnläggningsSoftType = new SoftType_FunktionellAnläggning
      {
        Array = true,
        id = "FunktionellAnläggning",
        instances = plattformsFunktioner.ToArray()
      };
      var anläggningsProduktSoftTypeSkydd = new SoftType_Anläggningsprodukt
      {
        Array = true,
        id = "Anläggningsprodukt",
        instances = plattformVäderskydd.ToArray()
      };
      var styckevaraProduktSoftType = new SoftType_Styckevara
      {
        Array = true,
        id = "Styckevara",
        instances = plattformStyckevaror.ToArray()
      };
      var anläggningsProduktSTSkylt = new SoftType_Anläggningsprodukt
      {
        Array = true,
        id = "Anläggningsprodukt",
        instances = plattformSkylt.ToArray()
      };
      var styckevaraProduktSTSkylt = new SoftType_Styckevara
      {
        Array = true,
        id = "Styckevara",
        instances = plattformSkyltIndivid.ToArray()
      };
      var anläggningsProduktKanalisation = new SoftType_Anläggningsprodukt
      {
        Array = true,
        id = "Anläggningsprodukt",
        instances = plattformKanalisation.ToArray()
      };
      var styckevaraProduktKanalisation = new SoftType_Styckevara
      {
        Array = true,
        id = "Styckevara",
        instances = plattformKanalisationIndivid.ToArray()
      };
      var plattformIndivid = new SoftType_Styckevara
      {
        Array = true,
        id = "Styckevara",
        instances = plattformIndivider.ToArray()
      };

      containerSoftTypes.Add(anläggningsProduktSoftType);
      containerSoftTypes.Add(funktionellAnläggningsSoftType);
      containerSoftTypes.Add(anläggningsProduktSoftTypeSkydd);
      containerSoftTypes.Add(styckevaraProduktSoftType);
      containerSoftTypes.Add(anläggningsProduktSTSkylt);
      containerSoftTypes.Add(styckevaraProduktSTSkylt);
      containerSoftTypes.Add(anläggningsProduktKanalisation);
      containerSoftTypes.Add(styckevaraProduktKanalisation);
      containerSoftTypes.Add(plattformIndivid);

      //Adds the extra softypes needed
      containerSoftTypes.AddRange(CreateSupplementarySoftypes());
      //containerSoftTypes.AddRange(CreateKeyReferences());

      containerSoftTypes.AddRange(CreateFTKeyReferenceSoftTypes());

      //Last step is to prepare the container for serialization
      container.softTypes = containerSoftTypes.ToArray();
      return container;
    }

    public override List<SoftType> CreateFTKeyReferenceSoftTypes()
    {
      var softtypeList = new List<SoftType>();
      //TODO: LÄGG TILL GPR UPPE OCH NERE

      //FTAnläggningsProdukter 
      var plattformProdukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "Plattformprodukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "Plattformprodukt",
          name = FeatureTypeName
        }
      };

      var väderskyddProdukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "Väderskyddprodukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "Väderskyddprodukt",
          name = FeatureTypeName
        }
      };

      var skärmtakProdukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "Skärmtakprodukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "Skärmtakprodukt",
          name = FeatureTypeName
        }
      };
      
      var skyltProdukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "Fast_skyltprodukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "Fast_skyltprodukt",
          name = FeatureTypeName
        }
      };
      
      var kanalisationprodukt = new FTAnläggningsproduktEntrydefaultIn
      {
        Array = true,
        id = "Kanalisationprodukt",
        inputSchemaRef = _InputSchemaRef,
        data = new FTAnläggningsproduktdefaultIn
        {
          id = "Kanalisationprodukt",
          //SPECIAL NAME
          name = "Läst från fil BIS_Kanalisation - datadefinition infomod 2p0.xlsm BIS_Plattform - datadefinition infomod 2p0.xlsm"
        }
      };

      var FTAnläggningsproduktInstances = new List<FTAnläggningsproduktInstances>
      { plattformProdukt, väderskyddProdukt, skärmtakProdukt, skyltProdukt, kanalisationprodukt };
      var FTAnläggningsProduktSoftType = new SoftType_FTAnläggningsprodukt
      {
        Array = true,
        id = "FTAnläggningsprodukt",
        instances = FTAnläggningsproduktInstances.ToArray()
      };
      softtypeList.Add(FTAnläggningsProduktSoftType);
      //FTAnläggningsProdukter END

      //FTStyckevaror
      
      var väderskyddindivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "Väderskyddindivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "Väderskyddindivid",
          name = FeatureTypeName
        }
      };
      
      var skärmtakindivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "Skärmtakindivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "Skärmtakindivid",
          name = FeatureTypeName
        }
      };

      var kanalisationindivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "Kanalisationindivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "Kanalisationindivid",
          name = FeatureTypeName
        }
      };

      var plattformindivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "Plattformindivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "Plattformindivid",
          name = FeatureTypeName
        }
      };
      
      var skyltindivid = new FTStyckevaraEntrydefaultIn
      {
        Array = true,
        id = "Fast_skyltindivid",
        inputSchemaRef = _InputSchemaRef,
        data = new FTStyckevaradefaultIn
        {
          id = "Fast_skyltindivid",
          name = FeatureTypeName
        }
      };

      var FTStyckevaraInstances = new List<FTStyckevaraInstances>
      { väderskyddindivid, skärmtakindivid, kanalisationindivid, plattformindivid, skyltindivid };
      var FTStyckevaraSoftType = new SoftType_FTStyckevara
      {
        Array = true,
        id = "FTStyckevara",
        instances = FTStyckevaraInstances.ToArray()
      };
      softtypeList.Add(FTStyckevaraSoftType);
      //FTStyckevara END

      //FTFunktionellAnläggning
      var plattformfunktion = new FTFunktionellAnläggningEntrydefaultIn
      {
        Array = true,
        id = "Plattformfunktion",
        inputSchemaRef = _InputSchemaRef,
        data = new FTFunktionellAnläggningdefaultIn
        {
          id = "Plattformfunktion",
          name = FeatureTypeName
        }
      };

      var FTFunktionellAnläggningInstances = new List<FTFunktionellAnläggningInstances> { plattformfunktion };
      var FTFunktionellAnläggningSoftType = new SoftType_FTFunktionellAnläggning
      {
        Array = true,
        id = "FTFunktionellAnläggning",
        instances = FTFunktionellAnläggningInstances.ToArray()
      };
      softtypeList.Add(FTFunktionellAnläggningSoftType);
      //FTFunktionellAnläggning END

      return softtypeList;
    }

    #region PlattformPropertyTranslators
    private Plattformfunktion_harSkyddsräcken SkapaPlattformSkyddsräcken(BIS_Plattform bisPlattform, Plattformfunktion_harSkyddsräcken plattformSkyddsräcken)
    {
      plattformSkyddsräcken.generalProperty = new harSkyddsräcken
      {
        softType = _SoftTypeProperty,
        instanceRef = "harSkyddsräcken"
      };
      plattformSkyddsräcken.value = bisPlattform.Skyddsräcken == "?" ? "Okänt" : bisPlattform.Skyddsräcken;
      return plattformSkyddsräcken;
    }

    private Plattformfunktion_harPlattformsutrustning SkapaPlattformUtrustning(BIS_Plattform bisPlattform, Plattformfunktion_harPlattformsutrustning plattformUtrustning)
    {
      plattformUtrustning.generalProperty = new harPlattformsutrustning
      {
        softType = _SoftTypeProperty,
        instanceRef = "harPlattformsutrustning"
      };
      plattformUtrustning.value = bisPlattform.PlattformsUtrustning == "?" ? "Okänt" : bisPlattform.PlattformsUtrustning;
      plattformUtrustning.JSonMapToPropertyName = _JsonMapToValue;
      return plattformUtrustning;
    }

    private Plattformfunktion SkyddzonOchLedstråk(BIS_Plattform bisPlattform, Plattformfunktion plattformFunktion, Plattformfunktion_harLedstråk plattformLedstråk, Plattformfunktion_harSkyddszon plattformSkyddszon)
    {
      plattformLedstråk.generalProperty = new harLedstråk
      {
        softType = _SoftTypeProperty,
        instanceRef = "harLedstråk"
      };
      plattformLedstråk.JSonMapToPropertyName = _JsonMapToValue;
      plattformSkyddszon.generalProperty = new harSkyddszon
      {
        softType = _SoftTypeProperty,
        instanceRef = "harSkyddzon"
      };
      plattformSkyddszon.JSonMapToPropertyName = _JsonMapToValue;

      switch (bisPlattform.Skyddszon_Och_Ledstråk)
      {
        case "S & L":
          plattformLedstråk.value = "Ja";
          plattformSkyddszon.value = "Ja";
          break;
        case "L":
          plattformLedstråk.value = "Ja";
          plattformSkyddszon.value = "Nej";
          break;
        case "S":
          plattformLedstråk.value = "Nej";
          plattformSkyddszon.value = "Ja";
          break;
        default:
          plattformLedstråk.value = "Okänt";
          plattformSkyddszon.value = "Okänt";
          break;
      }
      plattformFunktion.stringSet.harLedstråk = plattformLedstråk;
      plattformFunktion.stringSet.harSkyddszon = plattformSkyddszon;
      return plattformFunktion;
    }

    private Plattformfunktion_harFotsteg SkapaPlattformFotsteg(BIS_Plattform bisPlattform, Plattformfunktion_harFotsteg plattformFotsteg)
    {
      plattformFotsteg.generalProperty = new harFotsteg
      {
        softType = _SoftTypeProperty,
        instanceRef = "harFotsteg"
      };
      plattformFotsteg.value = bisPlattform.Fotsteg == "?" ? "Okänt" : bisPlattform.Fotsteg;
      plattformFotsteg.JSonMapToPropertyName = _JsonMapToValue;
      return plattformFotsteg;
    }

    private Plattformprodukt_verkligHöjd SkapaVerkligHöjd(BIS_Plattform bisPlattform, Plattformprodukt_verkligHöjd plattformVerkligHöjd)
    {
      plattformVerkligHöjd.generalProperty = new verkligHöjd
      {
        softType = _SoftTypeProperty,
        instanceRef = "verkligHöjd"
      };

      plattformVerkligHöjd.JSonMapToPropertyName = _JsonMapToValue;

      switch (bisPlattform.Höjd)
      {
        case "låg":
          plattformVerkligHöjd.value = 0.35M;
          break;
        case "mellan":
          plattformVerkligHöjd.value = 0.58M;
          break;
        case "hög":
          plattformVerkligHöjd.value = 0.73M;
          break;
        case "TSD Hög":
          plattformVerkligHöjd.value = 0.75M;
          break;
        case "TSD Mellan":
          plattformVerkligHöjd.value = 0.55M;
          break;
      }
      plattformVerkligHöjd.Unit = new m
      {
        softType = "Unit",
        instanceRef = "m"
      };

      return plattformVerkligHöjd;
    }

    private Plattformprodukt_plattformskantMaterial SkapaPlattformKantMaterial(BIS_Plattform bisPlattform, Plattformprodukt_plattformskantMaterial plattformskantMaterial)
    {
      plattformskantMaterial.generalProperty = new plattformskantMaterial
      {
        softType = _SoftTypeProperty,
        instanceRef = "plattformskantMaterial"
      };
      plattformskantMaterial.value = bisPlattform.Plattformskant_mtrl == "?" ? "Okänt" : bisPlattform.Plattformskant_mtrl;
      plattformskantMaterial.JSonMapToPropertyName = _JsonMapToValue;
      return plattformskantMaterial;
    }

    private Plattformprodukt_PlattformBeläggning SkapaPlattformBeläggning(BIS_Plattform bisPlattform, Plattformprodukt_PlattformBeläggning plattformBeläggning)
    {
      plattformBeläggning.generalProperty = new PlattformBeläggning
      {
        softType = _SoftTypeProperty,
        instanceRef = "PlattformBeläggning"
      };
      plattformBeläggning.value = bisPlattform.Beläggning == "?" ? "Okänt" : bisPlattform.Beläggning;
      plattformBeläggning.JSonMapToPropertyName = _JsonMapToValue;
      return plattformBeläggning;
    }
    #endregion
  }
}
