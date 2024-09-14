using System.Xml.Serialization;

namespace ImportXMLData.Domain.XmlDeserializerDomain
{
    [Serializable]
    public class XmlOrder
    {
        [XmlElement("no")]
        public int No { get; set; }

        [XmlElement("reg_date")]
        public DateOnly RegDate { get; set; }

        [XmlElement("product")]
        public List<XmlProduct> Products { get; set; } = new List<XmlProduct>();

        [XmlElement("user")]
        public XmlUser User { get; set; }
    }
}
