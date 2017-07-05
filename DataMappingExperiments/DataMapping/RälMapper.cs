using System.Collections.Generic;
using System.Linq;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments.DataMapping
{
  public class RälMapper : Mapper
  {
    public RälMapper()
    {
      MapperType = MapperType.Räl;
    }
    public sealed override MapperType MapperType { get; set; }
    public override BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt bisObject)
    {
      var myRäl = (BIS_Räl)bisObject;

      switch (index)
      {
        case 0:
          myRäl.ObjektTypNummer = attributeValue;
          break;
        case 1:
          myRäl.ObjektNummer = attributeValue;
          break;
        case 29:
          myRäl.SidaHVB = attributeValue;
          break;
        case 30:
          myRäl.Rälmodell = attributeValue;
          break;
        case 31:
          myRäl.Vikt = attributeValue;
          break;
        case 32:
          myRäl.Längd = attributeValue;
          break;
        case 33:
          myRäl.Skarvtyp = attributeValue;
          break;
        case 34:
          myRäl.Inläggningsår = attributeValue;
          break;
        case 35:
          myRäl.Tillverkngingsår = attributeValue;
          break;
        case 36:
          myRäl.Rev_Klass = attributeValue;
          break;
        case 37:
          myRäl.Tillverkare = attributeValue;
          break;
        case 38:
          myRäl.Stålsort = attributeValue;
          break;
        case 39:
          myRäl.Tillv_Process = attributeValue;
          break;
        case 40:
          myRäl.Notering = attributeValue;
          break;
        case 41:
          myRäl.Senast_Ändrad = attributeValue;
          break;
        case 42:
          myRäl.Senast_Ändrad_Av = attributeValue;
          break;
      }
      return myRäl;
    }

    public override Container ObjectStructure(List<BIS_GrundObjekt> bisList)
    {
      //TODO: Temporary list reform
      var formattedBisList = SquashTheList(bisList);
      Container container = new Container();
      var containerSoftypes = new List<SoftType>();
      var räls = new List<FunktionellAnläggningInstances>();

      foreach (BIS_Räl bisRäl in formattedBisList)
      {
        var rälsInstans = new FunktionellAnläggningEntrydefaultIn
        {
          Array = true,
          id = "Räl"
        };

        Räl räl = new Räl
        {
          notering = bisRäl.Notering,
          name = "",
          versionId = "",
          företeelsetyp = new ClassificationReference_FunktionellAnläggning_företeelsetyp
          {
            startSpecified = false,
            endSpecified = false,
            @class = new FTFunktionellAnläggningReference
            {
              instanceRef = "Räl",
              softType = "FTFunktionellAnläggningReference"
            }
          },
          stringSet = new RälStringSet
          {
            Skarvtyp = SkapaSkarvTyp(bisRäl, new Räl_Skarvtyp()),
            Stålsort = SkapaRälStålsort(bisRäl, new Räl_Stålsort()),
            Tillvprocess = SkapaTillverkningsProcess(bisRäl, new Räl_Tillvprocess()),
            Längdm = new Räl_Längdm
            {
              generalProperty = new Längdm
              {
                instanceRef = "Längd_x0028_m_x0029_",
                softType = _property
              },
              value = bisRäl.Längd,
              JSonMapToPropertyName = _value
            },
            Revklass = new Räl_Revklass
            {
              generalProperty = new Revklass
              {
                instanceRef = "Rev.klass",
                softType = _property
              },
              value = bisRäl.Rev_Klass,
              JSonMapToPropertyName = _value
            },
            Rälmodell = new Räl_Rälmodell
            {
              generalProperty = new Rälmodell
              {
                instanceRef = "Rälmodell",
                softType = _property
              },
              value = bisRäl.Rälmodell,
              JSonMapToPropertyName = _value
            },
            Tillverkare = new Räl_Tillverkare
            {
              generalProperty = new Tillverkare
              {
                instanceRef = "Tillverkare",
                softType = _property
              },
              value = bisRäl.Tillverkare,
              JSonMapToPropertyName = _value
            },
            sidahvb = new Räl_sidahvb
            {
              generalProperty = new sidahvb
              {
                instanceRef = "sida_x0020__x0028_h_x002C_v_x002C_b_x0029_",
                softType = _property
              },
              value = bisRäl.SidaHVB,
              JSonMapToPropertyName = _value
            }
          },
          numericSet = new RälNumericSet
          {
            Inläggningsår = new Räl_Inläggningsår
            {
              generalProperty = new Inläggningsår
              {
                instanceRef = "Inläggningsår",
                softType = _property
              },
              value = bisRäl.Inläggningsår,
              JSonMapToPropertyName = _value
            },
            Tillverkningsår = new Räl_Tillverkningsår
            {
              generalProperty = new Tillverkningsår
              {
                instanceRef = "Tillverkningsår",
                softType = _property
              },
              value = bisRäl.Tillverkngingsår,
              JSonMapToPropertyName = _value
            },
            Viktkgm = new Räl_Viktkgm
            {
              generalProperty = new Viktkgm
              {
                instanceRef = "Vikt_x0028_kg_x002F_m_x0029_",
                softType = _property
              },
              value = bisRäl.Vikt,
              JSonMapToPropertyName = _value
            }
          }
        };
        räl.id = räl.företeelsetyp.@class.instanceRef + bisRäl.ObjektTypNummer + bisRäl.ObjektNummer;
        räl = PropertyRealization(räl);
        rälsInstans.data = räl;
        räls.Add(rälsInstans);
      }

      //TODO: Uses only the first räl
      räls.RemoveRange(1, räls.Count - 1);

      var funktionellAnläggningsSofttype = new SoftType_FunktionellAnläggning
      {
        Array = true,
        id = "FunktionellAnläggning",
        instances = räls.ToArray()
      };

      containerSoftypes.Add(funktionellAnläggningsSofttype);
      containerSoftypes.AddRange(CreateSupplementarySoftypes());
      containerSoftypes.AddRange(CreateKeyReferences());

      container.softTypes = containerSoftypes.ToArray();
      return container;
    }

    #region PropertyCreationMethods

    private Räl PropertyRealization(Räl räl)
    {
      räl.företeelsetyp = new ClassificationReference_FunktionellAnläggning_företeelsetyp
      {
        @class = new FTFunktionellAnläggningReference
        {
          softType = "FTFunktionellAnläggning",
          instanceRef = "FTFunktionellAnläggning"
        },
        startSpecified = false,
        endSpecified = false
      };

      var anläggningsSpec = new BreakdownElementRealization_FunktionellAnläggning_anläggningsspecifikation
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
      räl.anläggningsspecifikation = new[] { anläggningsSpec };

      var anläggningsUtrymme = new BreakdownElementRealization_FunktionellAnläggning_anläggningsutrymme
      {
        value = new AnläggningsutrymmeReference
        {
          softType = "Anläggningsutrymme",
          instanceRef = "Anläggningsutrymme"
        },
        Array = true,
        startSpecified = false,
        endSpecified = false
      };
      räl.anläggningsutrymme = new[] { anläggningsUtrymme };

      räl.datainsamling = new PropertyValueAssignment_FunktionellAnläggning_datainsamling
      {
        startSpecified = false,
        endSpecified = false,
        value = "Datainsamling"
      };

      var dokument = new DocumentReference_FunktionellAnläggning_dokument
      {
        Array = true,
        startSpecified = false,
        endSpecified = false,
        value = new DokumentReference
        {
          softType = "Dokument",
          instanceRef = "Dokument"
        }
      };
      räl.dokument = new[] { dokument };

      räl.företeelsetillkomst = new PropertyValueAssignment_FunktionellAnläggning_företeelsetillkomst
      {
        startSpecified = false,
        endSpecified = false,
        value = "Företeelsetillkomst"
      };

      räl.ursprung = new PropertyValueAssignment_FunktionellAnläggning_ursprung
      {
        startSpecified = false,
        endSpecified = false,
        value = ""
      };

      var projekt = new ProjectReference_FunktionellAnläggning_projekt
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
      räl.projekt = new[] { projekt };
      return räl;
    }

    private Räl_Tillvprocess SkapaTillverkningsProcess(BIS_Räl bisRäl, Räl_Tillvprocess rälTillvprocess)
    {
      Tillvprocess tillvprocess = new Tillvprocess
      {
        instanceRef = "Tillv.process",
        softType = _property
      };
      rälTillvprocess.generalProperty = tillvprocess;
      rälTillvprocess.JSonMapToPropertyName = _value;

      switch (bisRäl.Tillv_Process)
      {
        case "E":
          rälTillvprocess.value = "Elektro";
          break;
        case "M":
          rälTillvprocess.value = "Martin";
          break;
        case "S":
          rälTillvprocess.value = "Syrgasprocess";
          break;
        case "T":
          rälTillvprocess.value = "Thomas";
          break;
        default:
          rälTillvprocess.value = bisRäl.Tillv_Process;
          break;
      }
      return rälTillvprocess;
    }

    private Räl_Skarvtyp SkapaSkarvTyp(BIS_Räl bisRäl, Räl_Skarvtyp rälSkarvtyp)
    {
      Skarvtyp skarvtyp = new Skarvtyp
      {
        instanceRef = "Skarvtyp",
        softType = _property
      };
      rälSkarvtyp.generalProperty = skarvtyp;
      rälSkarvtyp.JSonMapToPropertyName = _value;

      switch (bisRäl.Skarvtyp)
      {
        case "D":
          rälSkarvtyp.value = "Dubbelsliper(Skarvspår)";
          break;
        case "L":
          rälSkarvtyp.value = "Långräl (Helsvetsat)";
          break;
        case "S":
          rälSkarvtyp.value = "Svävande (Skarvspår)";
          break;
        case "T":
          rälSkarvtyp.value = "Treslipers (Skarvspår)";
          break;
        default:
          rälSkarvtyp.value = bisRäl.Skarvtyp;
          break;
      }
      return rälSkarvtyp;
    }

    private Räl_Stålsort SkapaRälStålsort(BIS_Räl bisRäl, Räl_Stålsort rälStålsort)
    {
      Stålsort stålsort = new Stålsort
      {
        instanceRef = "Stålsort",
        softType = _property
      };
      rälStålsort.generalProperty = stålsort;
      rälStålsort.JSonMapToPropertyName = _value;

      switch (bisRäl.Stålsort)
      {
        case "1100":
          rälStålsort.value = "R320Cr";
          break;
        case "800":
          rälStålsort.value = "R220";
          break;
        case "900A":
          rälStålsort.value = "R260";
          break;
        case "900B":
          rälStålsort.value = "900B";
          break;
        case "Cr":
          rälStålsort.value = "R320Cr";
          break;
        case "HT":
          rälStålsort.value = "R350HT";
          break;
        case "LHT":
          rälStålsort.value = "R350LHT";
          break;
        default:
          rälStålsort.value = bisRäl.Stålsort;
          break;
      }
      return rälStålsort;
    }

    #endregion

    /// <summary>
    /// Temporary squashing of the list. Unika plattformar: utan versioner med olika nätanknytningar.
    /// </summary>
    /// <param name="bisList"></param>
    /// <returns></returns>
    private IEnumerable<BIS_Räl> SquashTheList(List<BIS_GrundObjekt> bisList)
    {
      var myList = new List<BIS_Räl>();

      foreach (var objekt in bisList)
        myList.Add(objekt as BIS_Räl);

      var query = myList.GroupBy(objekt => objekt.ObjektTypNummer, (typNummer, notering) => new BIS_Räl
      {
        ObjektTypNummer = typNummer,
        Notering = string.Join(" ", notering.Select(x => x.Notering).OrderBy(x => x))
      });

      return query.GroupBy(objektDetalj => objektDetalj.ObjektNummer)
        .Select(values => values.FirstOrDefault());
    }
  }
}
