using ImportXMLData.Domain.XmlDeserializerDomain;

namespace ImportXMLData.Map
{
    public interface IMapper
    {
        Task Map(List<XmlOrder> orders);
    }
}
