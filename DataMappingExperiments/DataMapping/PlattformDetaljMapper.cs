using System;
using System.Collections.Generic;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments.DataMapping
{
  public class PlattformDetaljMapper : Mapper
  {
    public PlattformDetaljMapper()
    {
      MapperType = MapperType.PlattformDetalj;
    }
    public sealed override MapperType MapperType { get; set; }
    public override string MapXmlAttribute(int index, string attributeValue)
    {
      return attributeValue;
    }

    public override BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt bisObject)
    {
      var myPlattformDetalj = (BIS_Plattform_Detalj) bisObject;

      switch (index)
      {
        case 0:
          myPlattformDetalj.ObjektTypNummer = attributeValue;
          break;
        case 1:
          myPlattformDetalj.ObjektNummer = attributeValue;
          break;
      }

      return myPlattformDetalj;
    }

    public override Container ObjectStructure(List<BIS_GrundObjekt> bisList)
    {
      Console.WriteLine("Not Yet Implemented");
      return null;
    }
  }
}
