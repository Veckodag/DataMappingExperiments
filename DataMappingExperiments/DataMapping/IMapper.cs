﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMappingExperiments.BisObjekt;
using DataMappingExperiments.Helpers;

namespace DataMappingExperiments.DataMapping
{
  public interface IMapper
  {
    MapperType MapperType { get; set; }
    void HelloFromThisMappingclass();
    string MapXmlAttribute(int index, string attributeValue);
    BIS_GrundObjekt MapXmlValue(int index, string attributeValue, BIS_GrundObjekt BisObject);
  }
}
