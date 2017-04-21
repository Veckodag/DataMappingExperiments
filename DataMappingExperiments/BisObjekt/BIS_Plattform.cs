﻿using System;

namespace DataMappingExperiments.BisObjekt
{
  
  public class BIS_Plattform : BIS_GrundObjekt
  {
    public string Senast_Ändrad { get; set; }
    public string Senast_Ändrad_Av { get; set; }
    public string Höjd { get; set; }
    public int Längd { get; set; }
    public string Bredd { get; set; }
    public string Plattformskant_mtrl { get; set; }
    public string Beläggning { get; set; }
    //I anda är de 2 booleans: harSkyddszon, harLedstråk
    public string Skyddszon_Och_Ledstråk{ get; set; }
    public string Väderskydd { get; set; }
    public string Skylt { get; set; }
    public string Fotsteg { get; set; }
    public string Brunn_Och_Lock { get; set; }
    public string Skyddsräcken { get; set; }
    public string PlattformsUtrustning { get; set; }
    public string BesiktningsKlass { get; set; }
    public string Notering { get; set; }
  }
}
