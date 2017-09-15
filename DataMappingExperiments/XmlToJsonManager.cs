using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using DataMappingExperiments.Helpers;
using Newtonsoft.Json;
using TRV.ANDA.DataExchange.Toolbox.SasXml.Convert.JsonCustomCodeGenerators;

namespace DataMappingExperiments
{
  public static class XmlToJsonManager
  {
    public static void XmlToJson(Container container)
    {
      Console.WriteLine("Exporting XML to Json...");

      XmlDocument document = new XmlDocument();

      Serialize(container, out document, @"inputschemasföreteelsetyperDx");
 
      string json = SerializeXmlNode.XmlToJson(document);
      if (Program.SelectedDataContainer.MapperType == MapperType.Plattform)
        json = PlattformJsonFix(json);

      using (Stream stream = new FileStream(StringManager.GetFilePathSetting(Program.SelectedDataContainer.Json), FileMode.Create))
      {
        using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
        {
          writer.Write(json);
        }
      }
      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine("Json file complete!");
      Console.ForegroundColor = ConsoleColor.Gray;


    }

    private static string PlattformJsonFix(string json)
    {
      Console.WriteLine("Initiating Plattform special formatting");

      return json;
    }

    public static void Serialize(object instance, out XmlDocument xmlDocument, string nameSpace = null)
    {
      if (instance == null) throw new ArgumentNullException(nameof(instance));
      StringBuilder sb = new StringBuilder();

      using (var textWriter = XmlWriter.Create(sb))
      {
        var serializer = string.IsNullOrEmpty(nameSpace)
          ? new XmlSerializer(instance.GetType())
          : new XmlSerializer(instance.GetType(), nameSpace);

        serializer.Serialize(textWriter, instance);
        xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(sb.ToString());
      }
    }
  }
}
