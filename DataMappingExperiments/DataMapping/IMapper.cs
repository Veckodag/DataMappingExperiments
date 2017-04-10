using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMappingExperiments.DataMapping
{
  public interface IMapper
  {
    void HelloFromThisMappingclass();
    string MapXmlAttribute(int index, string attributeValue);
    string MapXmlValue(int index);
  }
}
