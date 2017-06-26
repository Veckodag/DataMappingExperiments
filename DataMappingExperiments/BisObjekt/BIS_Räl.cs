using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMappingExperiments.BisObjekt
{
  public class BIS_Räl : BIS_GrundObjekt
  {
    public string Rälmodell { get; set; }
    public int Vikt { get; set; }
    public string Längd { get; set; }
    public string Skarvtyp { get; set; }
    public int Inläggningsår { get; set; }
    public int Tillverkngingsår { get; set; }
    public string Rev_Klass { get; set; }
    public string Tillverkare { get; set; }
    public string Stålsort { get; set; }
    public string Tillv_Process { get; set; }
  }
}
