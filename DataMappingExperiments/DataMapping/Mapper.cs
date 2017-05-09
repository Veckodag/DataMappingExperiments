using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments.DataMapping
{
  [Serializable]
  public abstract class Mapper : IMapper
  {
    public abstract MapperType MapperType { get; set; }
    public virtual string Name => "Mapper";
    public abstract string MapXmlAttribute(int index, string attributeValue);
    public abstract BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt BisObject);
    public abstract void ObjectStructure(List<BIS_GrundObjekt> bisList);

    public void serialization(Container container)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(Container));
      TextWriter tw = new StreamWriter(@"C:\Users\fresan\Documents\Mappning ANDA\plattform.xml");
      serializer.Serialize(tw, container);
    }

    public List<UnitInstances> CreateSoftTypeUnitsInstances()
    {
      var unitlist = new List<UnitInstances>();
      //The actual values could come from a config file.
      string[] unitNameString = {"mm", "Procent", "Grader", "st"};

      foreach (var unitName in unitNameString)
      {
        var instance = new UnitEntrydefaultIn
        {
          Array = true,
          id = unitName,
          inputSchemaRef = "defaultIn"
        };
        var unit = new UnitdefaultIn
        {
          acronym = unitName
        };
        instance.data = unit;
        unitlist.Add(instance);
      }

      return unitlist;
    }

    public List<PropertyInstances> CreateSoftTypePropertyInstances()
    {
      var propertyList = new List<PropertyInstances>();
      //TODO: Map the properties

      var instance = new PropertyEntrydefaultIn
      {
        Array = true,
        inputSchemaRef = "defaultIn"
      };

      return propertyList;
    }
  }
}
