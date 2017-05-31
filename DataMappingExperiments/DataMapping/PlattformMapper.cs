using System;
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
    }
    public sealed override MapperType MapperType { get; set; }
    public override string MapXmlAttribute(int index, string attributeValue)
    {
      return attributeValue;
    }
    public override BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt plattform)
    {
      var myPlattform = (BIS_Plattform)plattform;
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
      //TODO: Squash the list temporary
      var formattedBisList = SquashTheList(bisList);
      Container container = new Container();
      //All softypes must be aggregated before they are added to the container
      var containerSoftTypes = new List<SoftType>();
      var plattformar = new List<GeografiskPlaceringsreferensInstances>();

      //Does all the real mapping against ANDA resources
      foreach (BIS_Plattform bisPlattform in formattedBisList)
      {
        var plattformsInstans = new GeografiskPlaceringsreferensEntrydefaultIn
        {
          Array = true,
          id = "Plattform"
        };

        Plattform plattform = new Plattform
        {
          //TODO: Fixa de extraordinära properties
          id = Guid.NewGuid().ToString(),
          notering = bisPlattform.Notering,
          name = "",
          versionId = "",
          företeelsetyp = new ClassificationReference_GeografiskPlaceringsreferens_företeelsetyp
          {
            @class = new FTGeografiskPlaceringsreferensReference
            {
              instanceRef = "Plattform",
              softType = "FTGeografiskPlaceringsreferens"
            }
          },
          stringSet = new PlattformStringSet
          {
            start = "",
            end = "",
            Kmtalti = SkapaKmTali(bisPlattform, new Plattform_Kmtalti()),
            Kmtal = SkapaKmTal(bisPlattform, new Plattform_Kmtal()),
            Väderskydd = SkapaPlattformVäderskydd(bisPlattform, new Plattform_Väderskydd()),
            Beläggning = SkapaPlattformBeläggning(bisPlattform, new Plattform_Beläggning()),
            Brunnolock = SkapaPlattformBrunnOLock(bisPlattform, new Plattform_Brunnolock()),
            Fotsteg = SkapaPlattformFotsteg(bisPlattform, new Plattform_Fotsteg()),
            Höjd = SkapaPlattformHöjd(bisPlattform, new Plattform_Höjd()),
            Skyddsräcken = SkapaPlattformSkyddsräcken(bisPlattform, new Plattform_Skyddsräcken()),
            Skylt = SkapaPlattformSkylt(bisPlattform, new Plattform_Skylt()),
            Plattformsutrustning = SkapaPlattformsUtrustning(bisPlattform, new Plattform_Plattformsutrustning()),
            Plattformskantmtrl = SkapaPlattformsKantMaterial(bisPlattform, new Plattform_Plattformskantmtrl()),
            Skyddszonochledstråk = SkapaPlattformSkyddszonOchLedstråk(bisPlattform, new Plattform_Skyddszonochledstråk()),
            ObjektText = SkapaObjektText(bisPlattform, new Plattform_ObjektText()),
            Höjd_beskr = SkapaHöjdBeskrivning(bisPlattform, new Plattform_Höjd_beskr())
          },
          numericSet = new PlattformNumericSet
          {
            start = "",
            end = "",
            BisObjektNr = SkapaBisObjektNr(bisPlattform, new Plattform_BisObjektNr()),
            BisObjektTypNr = SkapaBisObjektTypNr(bisPlattform, new Plattform_BisObjektTypNr()),
            Breddm = SkapaPlattformBredd(bisPlattform, new Plattform_Breddm()),
            Längdm = SkapaPlattformLängd(bisPlattform, new Plattform_Längdm())
          }
        };
        //plattform = RealizationOfProperties(plattform);
        //add to list!
        plattformsInstans.data = plattform;
        plattformar.Add(plattformsInstans);
      }
      //Real Test
      //Add new softypes to containerSoftTypes
      //TODO: USES ONLY THE FIRST PLATTFORM AT THE MOMENT
      plattformar.RemoveRange(1, plattformar.Count - 1);
      var geografiskSofttype = new SoftType_GeografiskPlaceringsreferens
      {
        instances = plattformar.ToArray()
      };
      containerSoftTypes.Add(geografiskSofttype);
      containerSoftTypes.AddRange(CreateSupplementarySoftypes());

      //Last step is to prepare the container for serialization
      container.softTypes = containerSoftTypes.ToArray();
      return container;
    }

    private Plattform RealizationOfProperties(Plattform plattform)
    {
      plattform.arbetsnamn = "Arbetsnamn";

      //Anläggningsprodukt
      //Validation Problems for instanceRef in the references
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
      //Validation problems
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
      //Validation Problem
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
      //Validation Problems
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
      //Validation Problems
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
      //Validation Problems
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
      //Validation Problems
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
      //Validation problem
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

    /// <summary>
    /// Temporary squashing of the list. Unika plattformar utan versioner med olika nätanknytningar.
    /// </summary>
    /// <param name="bisList"></param>
    /// <returns></returns>
    private List<BIS_Plattform> SquashTheList(List<BIS_GrundObjekt> bisList)
    {
      var myList = new List<BIS_Plattform>();

      foreach (var objekt in bisList)
        myList.Add(objekt as BIS_Plattform);

      return myList.GroupBy(plattformDetalj => plattformDetalj.ObjektNummer)
        .Select(values => values.FirstOrDefault()).ToList();
    }

    private List<SoftType> CreateSupplementarySoftypes()
    {
      //TODO: Make the extraList complete
      var softtypeList = new List<SoftType>();
      var geografiskFTSofttype = new SoftType_FTGeografiskPlaceringsreferens
      {
        Array = true,
        id = "FTGeografiskPlaceringsreferens"
      };
      var FTPlattformar = new List<FTGeografiskPlaceringsreferensInstances>();
      //Vilka softtypes behövs
      var ftPlattformsInstans = new FTGeografiskPlaceringsreferensEntrydefaultIn
      {
        Array = true,
        id = "FTPlattform",
        inputSchemaRef = "defaultIn",
        data = new FTGeografiskPlaceringsreferensdefaultIn
        {
          id = "FTPlattform",
          name = "FTPlattform"
        }
      };
      FTPlattformar.Add(ftPlattformsInstans);
      geografiskFTSofttype.instances = FTPlattformar.ToArray();

      //TODO: Make Real Properties
      var softtypeProperty = new SoftType_Property
      {
        Array = true,
        id = "Property",
        instances = CreateSoftTypePropertyInstances().ToArray()
      };
      var softtypeUnit = new SoftType_Unit
      {
        Array = true,
        id = "Unit",
        instances = CreateSoftTypeUnitsInstances().ToArray()
      };

      //Add them all to the list
      softtypeList.Add(geografiskFTSofttype);
      softtypeList.Add(softtypeProperty);
      softtypeList.Add(softtypeUnit);

      return softtypeList;
    }

    #region PropertyCreationMethods

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
      plattformBisObjektTypNr.value = "10004";
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
    #endregion
  }
}
