using ImportXMLData.Domain.XmlDeserializerDomain;

namespace ImportXMLData.Validator
{
    public interface IDataValidator
    {
        void Validate(List<XmlOrder> orders);
    }
}
