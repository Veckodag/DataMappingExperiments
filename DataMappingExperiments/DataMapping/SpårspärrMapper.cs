using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments.DataMapping
{
  public class SpårspärrMapper : Mapper
  {
    public SpårspärrMapper()
    {
      MapperType = MapperType.Spårspärr;
    }
    public sealed override MapperType MapperType { get; set; }
    public override BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt bisObject)
    {
      throw new NotImplementedException();
    }

    public override Container ObjectStructure(List<BIS_GrundObjekt> bisList)
    {
      throw new NotImplementedException();
    }
  }
}
