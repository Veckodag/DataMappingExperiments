﻿using System.Collections.Generic;
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
      var formattedBisList = SquashTheList(bisList);
      Container container = new Container();
      //All softypes must be aggregated before they are added to the container
      var containerSoftTypes = new List<SoftType>();

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
        //TODO: Figure out what the plattform does
        var masterPlattform = new Plattform()
        {

        };

        var plattformsProduktInstans = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          id = "PlattformProdukt",
          inputSchemaRef = _InputSchemaRef
        };

        Plattformprodukt plattformprodukt = new Plattformprodukt
        {
          notering = bisPlattform.Notering,
          name = "Plattformprodukt",
          versionId = "001",
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
          }
        };
        plattformprodukt.id = plattformprodukt.name + bisPlattform.ObjektTypNummer + bisPlattform.ObjektNummer;

        plattformprodukt = PlattformProduktPropertyRealization(plattformprodukt);
        plattformsProduktInstans.data = plattformprodukt;
        plattformsProdukter.Add(plattformsProduktInstans);

        var plattformsFunktionInstans = new FunktionellAnläggningEntrydefaultIn
        {
          Array = true,
          id = "PlattformFunktion",
          inputSchemaRef = "defaultIn"
        };

        var plattformFunktion = new Plattformfunktion
        {
          notering = bisPlattform.Notering,
          name = "Plattform",
          versionId = "001",
          stringSet = new PlattformfunktionStringSet
          {
            harFotsteg = SkapaPlattformFotsteg(bisPlattform, new Plattformfunktion_harFotsteg()),
            harPlattformsutrustning = SkapaPlattformUtrustning(bisPlattform, new Plattformfunktion_harPlattformsutrustning()),
            harSkyddsräcken = SkapaPlattformSkyddsräcken(bisPlattform, new Plattformfunktion_harSkyddsräcken())
          },
          numericSet = new PlattformfunktionNumericSet()
        };
        plattformFunktion = SkyddzonOchLedstråk(bisPlattform, plattformFunktion, new Plattformfunktion_harLedstråk(), new Plattformfunktion_harSkyddszon());

        plattformFunktion.id = plattformFunktion.name + bisPlattform.ObjektTypNummer + bisPlattform.ObjektNummer;
        plattformFunktion = PlattformFunktionPropertyRealization(plattformFunktion);
        plattformsFunktionInstans.data = plattformFunktion;
        plattformsFunktioner.Add(plattformsFunktionInstans);

        //Skydd eller Tak
        var plattformAnläggningsProduktInstans = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          id = "PlattformAnläggningsProdukt",
          inputSchemaRef = _InputSchemaRef
        };
        var plattformStyckevaraInstans = new StyckevaraEntrydefaultIn
        {
          Array = true,
          id = "PlattformStyckevara",
          inputSchemaRef = _InputSchemaRef
        };

        var väderskyddProdukt = new Väderskyddprodukt { name = "Väderskyddprodukt", notering = bisPlattform.Notering };
        väderskyddProdukt.id = väderskyddProdukt.name + bisPlattform.ObjektTypNummer + bisPlattform.ObjektNummer;
        var väderskyddIndivid = new Väderskyddindivid
        {
          startSpecified = false,
          endSpecified = false,
          name = "Väderskyddindivid",
          notering = bisPlattform.Notering
        };
        väderskyddIndivid.id = väderskyddIndivid.name + bisPlattform.ObjektTypNummer + bisPlattform.ObjektNummer;

        var skärmtakProdukt = new Skärmtakprodukt { name = "Skärmtakprodukt", notering = bisPlattform.Notering };
        skärmtakProdukt.id = skärmtakProdukt.name + bisPlattform.ObjektTypNummer + bisPlattform.ObjektNummer;
        var skärmtakIndivid = new Skärmtakindivid
        {
          startSpecified = false,
          endSpecified = false,
          name = "Skärmtakindivid",
          notering = bisPlattform.Notering
        };
        skärmtakIndivid.id = skärmtakIndivid.name + bisPlattform.ObjektTypNummer + bisPlattform.ObjektNummer;

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
        var skyltProdukt = new Fast_skyltprodukt { name = "SkyltProdukt", notering = bisPlattform.Notering };
        skyltProdukt.id = skyltProdukt.name + bisPlattform.ObjektTypNummer + bisPlattform.ObjektNummer;
        var skyltIndivid = new Fast_skyltindivid
        {
          startSpecified = false,
          endSpecified = false,
          name = "SkyltIndivid",
          notering = bisPlattform.Notering
        };
        skyltIndivid.id = skyltIndivid.name + bisPlattform.ObjektTypNummer + bisPlattform.ObjektNummer;

        var plattformSkyltAnläggningsProdukt = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          id = "PlattformAnläggningsProdukt",
          inputSchemaRef = _InputSchemaRef,
          data = skyltProdukt
        };
        var plattformSkyltStyckevara = new StyckevaraEntrydefaultIn
        {
          Array = true,
          id = "PlattformStyckevara",
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
          stringSet = new KanalisationproduktStringSet(),
          numericSet = new KanalisationproduktNumericSet()
        };
        kanalisationsProdukt.id = kanalisationsProdukt.name + bisPlattform.ObjektTypNummer + bisPlattform.ObjektNummer;
        var kanalisationsIndivid = new Kanalisationindivid { name = "Kanalisationindivid", notering = bisPlattform.Notering };
        kanalisationsIndivid.id = kanalisationsIndivid.name + bisPlattform.ObjektTypNummer + bisPlattform.ObjektNummer;

        var plattformKanalisationProdukt = new AnläggningsproduktEntrydefaultIn
        {
          Array = true,
          id = "PlattformAnläggningsProdukt",
          inputSchemaRef = _InputSchemaRef,
          data = kanalisationsProdukt
        };
        var plattformKanalisationStyckevara = new StyckevaraEntrydefaultIn
        {
          Array = true,
          id = "PlattformStyckevara",
          inputSchemaRef = _InputSchemaRef,
          data = kanalisationsIndivid
        };
        if (bisPlattform.Brunn_Och_Lock != "?")
        {
          plattformKanalisation.Add(plattformKanalisationProdukt);
          plattformKanalisationIndivid.Add(plattformKanalisationStyckevara);
        }

        //PlattformIndivid?!
        var plattformindivid = new Plattformindivid
        {
          startSpecified = false,
          endSpecified = false,
          name = "Plattformindivid",
          notering = bisPlattform.Notering
        };
        plattformindivid.id = plattformindivid.name + bisPlattform.ObjektTypNummer + bisPlattform.ObjektNummer;

        var plattformIndividStyckevara = new StyckevaraEntrydefaultIn
        {
          Array = true,
          id = "PlattformStyckevara",
          inputSchemaRef = _InputSchemaRef,
          data = plattformindivid
        };
        plattformIndivider.Add(plattformIndividStyckevara);

        //End of real plattform

        #region Old Plattform

        //Plattform plattform = new Plattform
        //{
        //  notering = bisPlattform.Notering,
        //  name = "Plattform",
        //  versionId = "001",
        //  företeelsetyp = new ClassificationReference_GeografiskPlaceringsreferens_företeelsetyp
        //  {
        //    startSpecified = false,
        //    endSpecified = false,
        //    @class = new FTGeografiskPlaceringsreferensReference
        //    {
        //      instanceRef = "Plattform",
        //      softType = "FTGeografiskPlaceringsreferens"
        //    }
        //  },
        //  stringSet = new PlattformStringSet
        //  {
        //    Kmtalti = SkapaKmTali(bisPlattform, new Plattform_Kmtalti()),
        //    Kmtal = SkapaKmTal(bisPlattform, new Plattform_Kmtal()),
        //    Väderskydd = SkapaPlattformVäderskydd(bisPlattform, new Plattform_Väderskydd()),
        //    Beläggning = SkapaPlattformBeläggning(bisPlattform, new Plattform_Beläggning()),
        //    Brunnolock = SkapaPlattformBrunnOLock(bisPlattform, new Plattform_Brunnolock()),
        //    Fotsteg = SkapaPlattformFotsteg(bisPlattform, new Plattform_Fotsteg()),
        //    Höjd = SkapaPlattformHöjd(bisPlattform, new Plattform_Höjd()),
        //    Skyddsräcken = SkapaPlattformSkyddsräcken(bisPlattform, new Plattform_Skyddsräcken()),
        //    Skylt = SkapaPlattformSkylt(bisPlattform, new Plattform_Skylt()),
        //    Plattformsutrustning = SkapaPlattformsUtrustning(bisPlattform, new Plattform_Plattformsutrustning()),
        //    Plattformskantmtrl = SkapaPlattformsKantMaterial(bisPlattform, new Plattform_Plattformskantmtrl()),
        //    Skyddszonochledstråk = SkapaPlattformSkyddszonOchLedstråk(bisPlattform, new Plattform_Skyddszonochledstråk()),
        //    ObjektText = SkapaObjektText(bisPlattform, new Plattform_ObjektText()),
        //    Höjd_beskr = SkapaHöjdBeskrivning(bisPlattform, new Plattform_Höjd_beskr())
        //  },
        //  numericSet = new PlattformNumericSet
        //  {
        //    BisObjektNr = SkapaBisObjektNr(bisPlattform, new Plattform_BisObjektNr()),
        //    BisObjektTypNr = SkapaBisObjektTypNr(bisPlattform, new Plattform_BisObjektTypNr()),
        //    Breddm = SkapaPlattformBredd(bisPlattform, new Plattform_Breddm()),
        //    Längdm = SkapaPlattformLängd(bisPlattform, new Plattform_Längdm())
        //  }
        //};
        //plattform.id = plattform.företeelsetyp.@class.instanceRef + bisPlattform.ObjektTypNummer +
        //               bisPlattform.ObjektNummer;
        //The Other Properties
        //plattform = PropertyRealization(plattform);

        //add to list!
        //plattformsInstans.data = plattform;
        //plattformar.Add(plattformsInstans);

        #endregion
      }
      //Real Test
      //Add new softypes to containerSoftTypes

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
      containerSoftTypes.AddRange(CreateKeyReferences());

      //Last step is to prepare the container for serialization
      container.softTypes = containerSoftTypes.ToArray();
      return container;
    }

    private Plattformfunktion PlattformFunktionPropertyRealization(Plattformfunktion plattformFunktion)
    {
      var anläggningsSpec = new BreakdownElementRealization_FunktionellAnläggning_anläggningsspecifikation
      {
        value = new AnläggningsspecifikationReference
        {
          softType = _SoftTypeProperty,
          instanceRef = "Anläggningsspecifikation"
        },
        Array = true,
        startSpecified = false,
        endSpecified = false
      };
      var anläggningsSpecLista =
        new List<BreakdownElementRealization_FunktionellAnläggning_anläggningsspecifikation> { anläggningsSpec };
      plattformFunktion.anläggningsspecifikation = anläggningsSpecLista.ToArray();

      var anläggningsUtrymme = new BreakdownElementRealization_FunktionellAnläggning_anläggningsutrymme
      {
        value = new AnläggningsutrymmeReference
        {
          softType = _SoftTypeProperty,
          instanceRef = "Anläggningsutrymme"
        },
        Array = true,
        startSpecified = false,
        endSpecified = false
      };
      var anläggningsUtrymmeLista = new List<BreakdownElementRealization_FunktionellAnläggning_anläggningsutrymme> { anläggningsUtrymme };
      plattformFunktion.anläggningsutrymme = anläggningsUtrymmeLista.ToArray();

      return plattformFunktion;
    }

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

    #region PlattformproduktPropertyCreation
    private Plattformprodukt PlattformProduktPropertyRealization(Plattformprodukt plattformprodukt)
    {
      //Datainsamling
      plattformprodukt.datainsamling = new PropertyValueAssignment_Anläggningsprodukt_datainsamling
      {
        startSpecified = false,
        endSpecified = false,
        value = "Datainsamling"
      };
      //Företeelsetillkomst
      plattformprodukt.företeelsetillkomst = new PropertyValueAssignment_Anläggningsprodukt_företeelsetillkomst
      {
        value = "Företeelsetillkomst",
        startSpecified = false,
        endSpecified = false
      };

      var projekt = new ProjectReference_Anläggningsprodukt_projekt
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = new ProjektReference
        {
          softType = "Projekt",
          instanceRef = "Projekt"
        }
      };
      var projektLista = new List<ProjectReference_Anläggningsprodukt_projekt> { projekt };
      plattformprodukt.projekt = projektLista.ToArray();

      plattformprodukt.ursprung = new PropertyValueAssignment_Anläggningsprodukt_ursprung
      {
        startSpecified = false,
        endSpecified = false,
        value = "Ursprung"
      };

      plattformprodukt.företeelsetyp = new ClassificationReference_Anläggningsprodukt_företeelsetyp
      {
        startSpecified = false,
        endSpecified = false,
        @class = new FTAnläggningsproduktReference
        {
          softType = "FTAnläggningsprodukt",
          instanceRef = "FTAnläggningsprodukt"
        }
      };
      //Dokument
      var dokument = new DocumentReference_Anläggningsprodukt_dokument()
      {
        value = new DokumentReference
        {
          softType = "Dokument",
          instanceRef = "Dokument"
        },
        Array = true,
        startSpecified = false,
        endSpecified = false
      };
      var dokumentLista = new List<DocumentReference_Anläggningsprodukt_dokument> { dokument };
      plattformprodukt.dokument = dokumentLista.ToArray();

      return plattformprodukt;
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
          //TODO: Andra cases
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
    #region PropertyCreationMethods

    private Plattform PropertyRealization(Plattform plattform)
    {
      plattform.arbetsnamn = "Arbetsnamn";

      //Anläggningsprodukt
      var anläggningsProdukt = new BreakdownElementRealization_GeografiskPlaceringsreferens_anläggningsprodukt
      {
        value = new AnläggningsproduktReference
        {
          softType = "Anläggningsprodukt",
          instanceRef = "Anläggningsprodukt"
        },
        Array = true,
        startSpecified = false,
        endSpecified = false
      };
      var anläggningsProduktLista =
        new List<BreakdownElementRealization_GeografiskPlaceringsreferens_anläggningsprodukt> { anläggningsProdukt };
      plattform.anläggningsprodukt = anläggningsProduktLista.ToArray();

      //Anläggningsspecifikation
      var anläggningsSpec = new BreakdownElementRealization_GeografiskPlaceringsreferens_anläggningsspecifikation
      {
        value = new AnläggningsspecifikationReference
        {
          softType = "Anläggningsspecifikation",
          instanceRef = "Anläggningsspecifikation"
        },
        Array = true,
        startSpecified = false,
        endSpecified = false
      };
      var anläggningsSpecLista =
        new List<BreakdownElementRealization_GeografiskPlaceringsreferens_anläggningsspecifikation> { anläggningsSpec };
      plattform.anläggningsspecifikation = anläggningsSpecLista.ToArray();

      //Bulkvara
      var bulkvara = new BreakdownElementRealization_GeografiskPlaceringsreferens_bulkvara
      {
        value = new BulkvaraReference
        {
          softType = "Bulkvara",
          instanceRef = "Bulkvara"
        },
        Array = true,
        startSpecified = false,
        endSpecified = false
      };
      var bulkvaraLista = new List<BreakdownElementRealization_GeografiskPlaceringsreferens_bulkvara> { bulkvara };
      plattform.bulkvara = bulkvaraLista.ToArray();

      //Dokument
      var dokument = new DocumentReference_GeografiskPlaceringsreferens_dokument
      {
        value = new DokumentReference
        {
          softType = "Dokument",
          instanceRef = "Dokument"
        },
        Array = true,
        startSpecified = false,
        endSpecified = false
      };
      var dokumentLista = new List<DocumentReference_GeografiskPlaceringsreferens_dokument> { dokument };
      plattform.dokument = dokumentLista.ToArray();

      //Företeelsetyp
      var företeelsetyp = new ClassificationReference_GeografiskPlaceringsreferens_företeelsetyp
      {
        startSpecified = false,
        endSpecified = false,
        @class = new FTGeografiskPlaceringsreferensReference
        {
          softType = "FTGeografiskPlaceringsreferens",
          instanceRef = "FTGeografiskPlaceringsreferens"
        }
      };
      plattform.företeelsetyp = företeelsetyp;

      //Konstaterad Tillståndsindivid
      var tillståndsIndivid = new BreakdownElementRealization_GeografiskPlaceringsreferens_konstateradTillståndsindivid
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = new KonstateradTillståndsindividReference
        {
          softType = "KonstateradTillståndsindivid",
          instanceRef = "KonstateradTillståndsindivid"
        }
      };
      var tillståndsindividLista =
        new List<BreakdownElementRealization_GeografiskPlaceringsreferens_konstateradTillståndsindivid> { tillståndsIndivid };
      plattform.konstateradTillståndsindivid = tillståndsindividLista.ToArray();

      //Styckevara
      var styckevara = new BreakdownElementRealization_GeografiskPlaceringsreferens_styckevara
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = new StyckevaraReference
        {
          softType = "Styckevara",
          instanceRef = "Styckevara"
        }
      };

      var styckevaraLista = new List<BreakdownElementRealization_GeografiskPlaceringsreferens_styckevara> { styckevara };
      plattform.styckevara = styckevaraLista.ToArray();

      //Projekt
      var projekt = new ProjectReference_GeografiskPlaceringsreferens_projekt
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = new ProjektReference
        {
          softType = "Projekt",
          instanceRef = "Projekt"
        }
      };

      var projektLista = new List<ProjectReference_GeografiskPlaceringsreferens_projekt> { projekt };
      plattform.projekt = projektLista.ToArray();

      //Ursprung
      plattform.ursprung = new PropertyValueAssignment_GeografiskPlaceringsreferens_ursprung
      {
        startSpecified = false,
        endSpecified = false,
        value = ""
      };

      //FöreteelseTillkomst
      var företeelsetillkomst = new PropertyValueAssignment_GeografiskPlaceringsreferens_företeelsetillkomst
      {
        value = "Företeelsetillkomst",
        startSpecified = false,
        endSpecified = false
      };
      plattform.företeelsetillkomst = företeelsetillkomst;

      //Datainsamling
      var datainsamling = new PropertyValueAssignment_GeografiskPlaceringsreferens_datainsamling
      {
        value = "Datainsamling",
        startSpecified = false,
        endSpecified = false
      };
      plattform.datainsamling = datainsamling;

      return plattform;
    }

    //PROPERTY CREATION

    /*
    private Plattform_Höjd_beskr SkapaHöjdBeskrivning(BIS_Plattform bisPlattform, Plattform_Höjd_beskr plattformHöjdBeskr)
    {
      Höjd_beskr höjdBeskr = new Höjd_beskr
      {
        instanceRef = "Höjd_beskr",
        softType = "Property"
      };
      plattformHöjdBeskr.generalProperty = höjdBeskr;
      plattformHöjdBeskr.value = "";
      plattformHöjdBeskr.JSonMapToPropertyName = "value";

      return plattformHöjdBeskr;
    }

    private Plattform_ObjektText SkapaObjektText(BIS_Plattform bisPlattform, Plattform_ObjektText plattformObjektText)
    {
      ObjektText objektText = new ObjektText
      {
        instanceRef = "ObjektText",
        softType = "Property"
      };
      plattformObjektText.generalProperty = objektText;
      plattformObjektText.value = "";
      plattformObjektText.JSonMapToPropertyName = "value";

      return plattformObjektText;
    }

    private Plattform_BisObjektTypNr SkapaBisObjektTypNr(BIS_Plattform bisPlattform, Plattform_BisObjektTypNr plattformBisObjektTypNr)
    {
      BisObjektTypNr bisObjektTypNr = new BisObjektTypNr
      {
        instanceRef = "BisObjektTypNr",
        softType = "Property"
      };
      plattformBisObjektTypNr.generalProperty = bisObjektTypNr;
      plattformBisObjektTypNr.value = bisPlattform.ObjektTypNummer;
      plattformBisObjektTypNr.JSonMapToPropertyName = "value";
      plattformBisObjektTypNr.Unit = new EmptyUnit();

      return plattformBisObjektTypNr;
    }

    private Plattform_BisObjektNr SkapaBisObjektNr(BIS_Plattform bisPlattform, Plattform_BisObjektNr plattformBisObjektNr)
    {
      BisObjektNr bisObjektNr = new BisObjektNr
      {
        instanceRef = "BisObjektNr",
        softType = "Property"
      };
      plattformBisObjektNr.generalProperty = bisObjektNr;
      plattformBisObjektNr.value = bisPlattform.ObjektNummer;
      plattformBisObjektNr.JSonMapToPropertyName = "value";
      plattformBisObjektNr.Unit = new EmptyUnit();

      return plattformBisObjektNr;
    }

    private Plattform_Kmtalti SkapaKmTali(BIS_Plattform bisPlattform, Plattform_Kmtalti plattformKmtalti)
    {
      Kmtalti kmtalti = new Kmtalti
      {
        instanceRef = "Kmtalti",
        softType = "Property"
      };
      plattformKmtalti.generalProperty = kmtalti;
      plattformKmtalti.value = bisPlattform.Kmtalti;
      plattformKmtalti.JSonMapToPropertyName = "value";

      return plattformKmtalti;
    }

    private Plattform_Kmtal SkapaKmTal(BIS_Plattform bisPlattform, Plattform_Kmtal plattformKmtal)
    {
      Kmtal kmtal = new Kmtal
      {
        instanceRef = "Kmtal",
        softType = "Property"
      };
      plattformKmtal.generalProperty = kmtal;
      plattformKmtal.value = bisPlattform.Kmtal;
      plattformKmtal.JSonMapToPropertyName = "value";

      return plattformKmtal;
    }

    private Plattform_Längdm SkapaPlattformLängd(BIS_Plattform bisPlattform, Plattform_Längdm plattformLängdm)
    {
      Längdm1 längd = new Längdm1
      {
        instanceRef = "Längd_x0020__x0028_m_x0029_",
        softType = "Property"
      };

      plattformLängdm.generalProperty = längd;
      plattformLängdm.value = bisPlattform.Längd;
      plattformLängdm.JSonMapToPropertyName = "value";
      plattformLängdm.Unit = new EmptyUnit();

      return plattformLängdm;
    }

    private Plattform_Breddm SkapaPlattformBredd(BIS_Plattform bisPlattform, Plattform_Breddm plattformBreddm)
    {
      Breddm bredd = new Breddm
      {
        instanceRef = "Bredd_x0020__x0028_m_x0029_",
        softType = "Property"
      };

      plattformBreddm.generalProperty = bredd;
      plattformBreddm.value = bisPlattform.Bredd;
      plattformBreddm.JSonMapToPropertyName = "value";
      plattformBreddm.Unit = new EmptyUnit();

      return plattformBreddm;
    }

    private Plattform_Skyddszonochledstråk SkapaPlattformSkyddszonOchLedstråk(BIS_Plattform bisPlattform, Plattform_Skyddszonochledstråk plattformSkyddszonochledstråk)
    {
      Skyddszonochledstråk skyddszonochledstråk = new Skyddszonochledstråk
      {
        instanceRef = "Skyddszon_x0020_och_x0020_ledstråk",
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
        instanceRef = "Plattformskant_x0020_mtrl",
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
        instanceRef = "Brunn_x0020_o_x0020_lock",
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

      plattformVäderskydd.generalProperty = väderskydd;
      plattformVäderskydd.value = bisPlattform.Väderskydd;

      plattformVäderskydd.JSonMapToPropertyName = "value";

      return plattformVäderskydd;
    }
    */
    #endregion

    /// <summary>
    /// Temporary squashing of the list. Unika plattformar: utan versioner med olika nätanknytningar.
    /// </summary>
    /// <param name="bisList"></param>
    /// <returns></returns>
    public override IEnumerable<BIS_GrundObjekt> SquashTheList(List<BIS_GrundObjekt> bisList)
    {
      var myList = new List<BIS_Plattform>();

      foreach (var objekt in bisList)
        myList.Add(objekt as BIS_Plattform);

      return myList.GroupBy(plattformDetalj => plattformDetalj.ObjektNummer)
        .Select(values => values.FirstOrDefault()).ToList();
    }
  }
}
