using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
      var myRäl = (BIS_Räl) bisObject;

      switch (index)
      {
        case 0:
          myRäl.ObjektTypNummer = attributeValue;
          break;
        case 1:
          myRäl.ObjektNummer = attributeValue;
          break;
        case 30:
          myRäl.Rälmodell = attributeValue;
          break;
        case 31:
          myRäl.Vikt = int.Parse(attributeValue, CultureInfo.InvariantCulture);
          break;
        case 32:
          myRäl.Längd = attributeValue;
          break;
        case 33:
          myRäl.Skarvtyp = attributeValue;
          break;
        case 34:
          myRäl.Inläggningsår = int.Parse(attributeValue, CultureInfo.InvariantCulture);
          break;
        case 35:
          myRäl.Tillverkngingsår = int.Parse(attributeValue, CultureInfo.InvariantCulture);
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
      Container container = new Container();
      var containerSoftypes = new List<SoftType>();

      return container;
    }
  }
}
