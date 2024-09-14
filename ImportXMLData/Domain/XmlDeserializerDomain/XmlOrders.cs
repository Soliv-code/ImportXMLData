using System.Xml.Serialization;

namespace ImportXMLData.Domain.XmlDeserializerDomain
{
    [Serializable]
    [XmlRoot("orders", Namespace = "", IsNullable = false)]
    public class XmlOrders
    {
        [XmlElement("order")]
        public List<XmlOrder> _xmlOrders { get; set; } = new List<XmlOrder>();
    }
}
