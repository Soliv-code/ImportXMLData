using ImportXMLData.Deserializer;
using ImportXMLData.Domain.XmlDeserializerDomain;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
namespace ImportXMLData.Tests
{
    public class XmlDeserializerTests
    {
        private readonly IXmlDeserializer _deserializer;
        public XmlDeserializerTests()
        {
            _deserializer = new XmlDeserializer();
        }
        [Fact]
        public void Deserialize_ValidXml_ReturnsOrdersList()
        {
            // Arrange
            string xmlContent = @"
                <orders>
                    <order>
                        <no>1</no>
                        <reg_date>2012-12-19</reg_date>
                        <product>
                            <quantity>2</quantity>
                            <name>LG 1755</name>
                            <price>12000.75</price>
                        </product>
                        <user>
                            <fio>Иванов Иван Иванович</fio>
                            <email>abc@email.com</email>
                        </user>
                    </order>
                </orders>";
            string tempFilePath = Path.GetTempFileName();
            File.WriteAllText(tempFilePath, xmlContent);

            // Act
            List<XmlOrder> orders = _deserializer.Deserialize(tempFilePath);

            // Assert
            orders.Should().HaveCount(1);
            var order = orders.First();
            order.No.Should().Be(1);
            //order.RegDate.Should().Be(DateOnly.Parse("2012-12-19"));
            order.Products.Should().HaveCount(1);
            order.User.Fio.Should().Be("Иванов Иван Иванович");
            order.User.Email.Should().Be("abc@email.com");

            // Cleanup
            File.Delete(tempFilePath);
        }
        [Fact]
        public void Deserialize_InvalidXml_ThrowsException()
        {
            // Arrange
            string invalidXmlContent = @"<invalid></xml>";
            string tempFilePath = Path.GetTempFileName();
            File.WriteAllText(tempFilePath, invalidXmlContent);

            // Act
            Action act = () => _deserializer.Deserialize(tempFilePath);

            // Assert
            act.Should().Throw<InvalidOperationException>();

            // Cleanup
            File.Delete(tempFilePath);
        }
    }
}
