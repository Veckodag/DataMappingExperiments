using System;

namespace DataMappingExperiments.BisObjekt
{
  [Serializable]
  public class BIS_GrundObjekt
  {
    public string ObjektTypNummer { get; set; }
    public string ObjektNummer { get; set; }
    public string Senast_Ändrad { get; set; }
    public string Senast_Ändrad_Av { get; set; }
    public string Notering { get; set; }
  }
}
