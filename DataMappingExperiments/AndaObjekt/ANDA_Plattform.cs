﻿using System;
using System.Xml.Serialization;

namespace DataMappingExperiments.AndaObjekt
{
  [Serializable]
  [XmlType(Namespace = "http://trafikverket.se/anda/inputschemasföreteelsetyperDx/20170316")]
  [XmlRoot(Namespace = "http://trafikverket.se/anda/inputschemasföreteelsetyperDx/20170316", IsNullable = false, ElementName = "Plattform")]
  public class ANDA_Plattform : Plattform
  {
    
  }
  [Serializable]
  public class ANDA_PlattformNumericSet : PlattformNumericSet
  {
    public decimal Verklighöjd { get; set; }
  }
  [Serializable]
  public class ANDA_PlattformStringSet : PlattformStringSet
  {
    public string Nominellhöjd { get; set; }
    public ANDA_Väderskydd AndaVäderskydd { get; set; }
    public ANDA_Skärmtak AndaSkärmtak { get; set; }
    public string HarSkyddzon { get; set; }
    public string HarLedstråk { get; set; }
    public ANDA_Skylt AndaSkylt { get; set; }
    public ANDA_BrunnOLock AndaBrunnOLock { get; set; }
  }

  public class ANDA_BrunnOLock
  {
    public string Kanalisationprodukt { get; set; }
    public string Kanalisationindivid => "";
    public string KanalisationPlacering => "";
  }

  public class ANDA_Skylt
  {
    public string Fast_Skyltprodukt { get; set; }
    public string Fast_Skyltindivid => "";
    public string Fast_SkyltPlacering => "";
  }

  public class ANDA_Skärmtak  
  {
    public string Skärmtakprodukt { get; set; }
    public string Skärmtakindivid => "";
    public string SkärmtakPlacering => "";
  }
  public class ANDA_Väderskydd
  {
    public string Väderskyddprodukt { get; set; }
    public string Väderskyddindivid => "";
    public string VäderskyddPlacering => "";

  }
}
