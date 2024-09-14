using System.Xml.Serialization;

namespace ImportXMLData.Domain.XmlDeserializerDomain
{
    public class XmlProduct
    {
        [XmlElement("quantity")]
        public int Quantity { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }
    }
}
