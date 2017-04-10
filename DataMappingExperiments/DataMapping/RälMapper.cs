using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMappingExperiments.DataMapping
{
  public class RälMapper : Mapper
  {

    public override void HelloFromThisMappingclass()
    {
      Console.WriteLine("I'am a Räl mapper!");
    }
    public override string MapXmlAttribute(int index, string attributeValue)
    {
      throw new NotImplementedException();
    }

    public override string MapXmlValue(int index)
    {
      throw new NotImplementedException();
    }
  }
}
