using System;
using System.Collections.Generic;
using System.Linq;
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
      //Experiment 1
      //Listformatting
      List<BIS_Plattform_Detalj> myList = new List<BIS_Plattform_Detalj>();
      foreach (var objekt in bisList)
        myList.Add(objekt as BIS_Plattform_Detalj);

      myList =
        myList.GroupBy(plattformDetalj => plattformDetalj.ObjektNummer)
          .Select(values => values.FirstOrDefault())
          .ToList();


      //Experiment 2
      //Find plattforms other mapping values or something

      Console.WriteLine("Not Yet Implemented");
      return null;
    }
  }
}
