using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments.DataMapping
{
  public class RälMapper : Mapper
  {
    private BIS_Räl _räl;

    public RälMapper()
    {
      _räl = new BIS_Räl();
      MapperType = MapperType.Räl;
    }
    public override MapperType MapperType { get; set; }
    public override string Name => "Räl";

    public override string MapXmlAttribute(int index, string attributeValue)
    {
      return attributeValue;
    }
    public override BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt BisObject)
    {
      throw new NotImplementedException();
    }
  }
}
