namespace DataMappingExperiments.BisObjekt
{
  public class BIS_SpårSpärr : BIS_GrundObjekt
  {
    public string SpårspärrNummer { get; set; }
    public string Modell { get; set; }
    public string Besiktningsklass { get; set; }
    public string TLSUrsprung { get; set; }
    public string Terminal { get; set; }
    public string TLSId { get; set; }
    public string TLSBeteckning { get; set; }
    public string TLSTyp { get; set; }
    public string CentralOmläggningsbar { get; set; }
    public string GårAttSpärraIStällv { get; set; }
    public string LokalfrigivBarIndivid { get; set; }
    public string Återgående { get; set; }
    public string FördFörÅtergång { get; set; }

    public string Nivå { get; set; }
    public string Komponent { get; set; }
    public string Position { get; set; }
    public string ModellTyp { get; set; }
    public string Artnr { get; set; }
    public string Inldat { get; set; }
  }
}
