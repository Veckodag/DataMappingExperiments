using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
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

      //using (Stream stream = new FileStream(StringManager.GetFilePathSetting(Program.JsonFile), FileMode.Create))
      //{
      //  using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
      //  {
      //    writer.Write(json);
      //  }
      //}
      Console.WriteLine("Json file complete!");
    }

    public static void Serialize(Object instance, out XmlDocument xmlDocument, string nameSpace = "")
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
