using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMappingExperiments.DataMapping
{
  public class PlattformMapper : Mapper 
  {
    public override void HelloFromThisMappingclass()
    {
      Console.WriteLine("I'am a plattform mapper!");
    }

    public override string MapXmlAttribute(int index, string attributeValue)
    {
      return attributeValue;
    }

    public override string MapXmlValue(int index)
    {
      throw new NotImplementedException();
    }
  }
}
