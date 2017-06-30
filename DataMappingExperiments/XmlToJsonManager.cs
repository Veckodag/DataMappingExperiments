using System;
using System.IO;
using System.Text;
using System.Xml;
using DataMappingExperiments.Helpers;
using TRV.ANDA.DataExchange.Toolbox.SasXml.Convert.JsonCustomCodeGenerators;

namespace DataMappingExperiments
{
  public static class XmlToJsonManager
  {
    public static void XmlToJson()
    {
      Console.WriteLine("Exporting XML to Json...");
      XmlDocument document = new XmlDocument();
      document.Load(StringManager.GetFilePathSetting(Program.PlattformOutput));

      string json = TRV.ANDA.DataExchange.Toolbox.SasXml.Convert.JsonCustomCodeGenerators.SerializeXmlNode.XmlToJson(document);

      using (Stream stream = new FileStream(StringManager.GetFilePathSetting(Program.JsonFile), FileMode.Create))
      {
        using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
        {
          writer.Write(json);
        }
      }
      Console.WriteLine("Json file complete!");
    }
  }
}
