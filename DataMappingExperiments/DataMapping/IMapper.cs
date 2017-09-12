using System.Collections.Generic;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments.DataMapping
{
  public interface IMapper
  {
    MapperType MapperType { get; set; }
    int ExtraCounter { get; set; }
    string FeatureTypeName { get; set; }
    BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt bisObject);
    Container ObjectStructure(List<BIS_GrundObjekt> bisList);
    List<SoftType> CreateFTKeyReferenceSoftTypes();
  }
}
