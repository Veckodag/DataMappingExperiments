using System;
using System.Web.UI.WebControls;

namespace DataMappingExperiments.DataMapping
{
  public abstract class Mapper : IMapper
  {
    public virtual void HelloFromThisMappingclass()
    {
      Console.WriteLine("I'am a normal mapper!");
    }

    public abstract string MapXmlAttribute(int index, string attributeValue);

    public abstract string MapXmlValue(int index);
  }
}
