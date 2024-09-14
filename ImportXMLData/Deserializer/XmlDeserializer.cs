using ImportXMLData.Domain.XmlDeserializerDomain;
using System.Xml.Serialization;

namespace ImportXMLData.Deserializer
{
    public class XmlDeserializer
    {
        public List<XmlOrder> Deserialize(string filePath)
        {
            var serializer = new XmlSerializer(typeof(XmlOrders));
            using (var reader = new StreamReader(filePath))
            {
                XmlOrders? xmlOrders = (XmlOrders)serializer.Deserialize(reader);
                return xmlOrders._xmlOrders;
            }
        }
    }
}
