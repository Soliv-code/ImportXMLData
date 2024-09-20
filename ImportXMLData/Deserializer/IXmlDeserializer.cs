using ImportXMLData.Domain.XmlDeserializerDomain;

namespace ImportXMLData.Deserializer
{
    public interface IXmlDeserializer
    {
        List<XmlOrder> Deserialize(string filePath);
    }
}
