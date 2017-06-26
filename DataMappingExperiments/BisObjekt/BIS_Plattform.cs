namespace DataMappingExperiments.BisObjekt
{

  public class BIS_Plattform : BIS_GrundObjekt
  {
    public string Höjd { get; set; }
    public decimal Längd { get; set; }
    public decimal Bredd { get; set; }
    public string Plattformskant_mtrl { get; set; }
    public string Beläggning { get; set; }
    public string Skyddszon_Och_Ledstråk { get; set; }
    public string Väderskydd { get; set; }
    public string Skylt { get; set; }
    public string Fotsteg { get; set; }
    public string Brunn_Och_Lock { get; set; }
    public string Skyddsräcken { get; set; }
    public string PlattformsUtrustning { get; set; }
    public string BesiktningsKlass { get; set; }
    public string Kmtal { get; set; }
    public string Kmtalti { get; set; }

    //Properties not currently used but might be!

    public string Plnfr { get; set; }
    public string Nodnrfr { get; set; }
    public string Plnrti { get; set; }
    public string Nodnrti { get; set; }
    public string Lnknr { get; set; }

  }
}
