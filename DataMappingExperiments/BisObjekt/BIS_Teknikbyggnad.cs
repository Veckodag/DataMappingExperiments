using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMappingExperiments.BisObjekt
{
  public class BIS_Teknikbyggnad : BIS_GrundObjekt
  {
    public string Ägare { get; set; }
    public string Beteckning { get; set; }
    public string Modell { get; set; }
    public string ByggnadsÅr { get; set; }
    public string Typ { get; set; }
    public string Fasadutförande { get; set; }
    public string Åskskydd { get; set; }
    public string OrdinarieNät { get; set; }
    public string RedundantNät { get; set; }
    public string HjälpkraftFaser { get; set; }
    public string HjälpkraftSäkring { get; set; }
    public string OrtsnätFaser { get; set; }
    public string OrtsnätSäkring { get; set; }
    public string OrtsnätIDNummer { get; set; }
    public string OrtsnätÄgare { get; set; }
    public string MellantrafoStorlek { get; set; }
    public string Sidoavstånd { get; set; }
    public string Northing { get; set; }
    public string Easting { get; set; }
    public string ReservelverkUtförande { get; set; }
    public string ReservelverkStorlek { get; set; }
    public string ReservelverkInstÅr { get; set; }
    public string ReservelverkTankvolym { get; set; }
    public string ReservelverkBatterimodell { get; set; }
    public string ReserveelverkBatteriInDatum { get; set; }
    public string Besktningsklass { get; set; }
    public string Komponent { get; set; }
    public string Placering { get; set; }
    public string ModellTyp { get; set; }
    public string Fabrikat { get; set; }
    public string SerieNummer { get; set; }
    public string Storlek { get; set; }
    public string InstDatum { get; set; }


  }
}
