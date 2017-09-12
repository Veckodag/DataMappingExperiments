namespace DataMappingExperiments.BisObjekt
{
  public class BIS_Kanalisation : BIS_GrundObjekt
  {
    public string Typ { get; set; }
    public string MaterialKanalisation { get; set; }
    public string MaterialLock { get; set; }
    public string DiameterBrunnRör { get; set; }
    public string BreddRänna { get; set; }
    public string AntalRör { get; set; }
  }
}
