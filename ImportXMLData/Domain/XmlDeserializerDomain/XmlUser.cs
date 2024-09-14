using System.Xml.Serialization;

namespace ImportXMLData.Domain.XmlDeserializerDomain
{
    public class XmlUser
    {
        [XmlElement("fio")]
        public string Fio { get; set; }

        [XmlElement("email")]
        public string Email { get; set; }
    }
}
