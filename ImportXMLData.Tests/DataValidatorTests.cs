using FluentAssertions;
using ImportXMLData.Domain.XmlDeserializerDomain;
using ImportXMLData.Validator;

namespace ImportXMLData.Tests
{
    public class DataValidatorTests
    {
        private readonly IDataValidator _validator;

        public DataValidatorTests()
        {
            _validator = new DataValidator();
        }

        [Fact]
        public void Validate_ValidOrders_DoesNotThrow()
        {
            // Arrange
            var orders = new List<XmlOrder>
            {
                new XmlOrder
                {
                    No = 1,
                    RegDate = DateOnly.Parse("2012-12-19"),
                    User = new XmlUser
                    {
                        Fio = "Иванов Иван Иванович",
                        Email = "abc@email.com"
                    },
                    Products = new List<XmlProduct>
                    {
                        new XmlProduct
                        {
                            Quantity = 2,
                            Name = "LG 1755",
                            Price = 12000.75m
                        }
                    }
                }
            };

            // Act
            Action act = () => _validator.Validate(orders);

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [MemberData(nameof(GetInvalidOrders))]
        public void Validate_InvalidOrders_ThrowsArgumentException(XmlOrder order, string expectedMessage)
        {
            // Arrange
            var orders = new List<XmlOrder> { order };

            // Act
            Action act = () => _validator.Validate(orders);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage(expectedMessage);
        }

        public static IEnumerable<object[]> GetInvalidOrders()
        {
            // Missing Order Number
            yield return new object[]
            {
                new XmlOrder
                {
                    No = 0, // Assuming No=0 is invalid
                    RegDate = DateOnly.Parse("2012-12-19"),
                    User = new XmlUser
                    {
                        Fio = "Иванов Иван Иванович",
                        Email = "abc@email.com"
                    },
                    Products = new List<XmlProduct>
                    {
                        new XmlProduct
                        {
                            Quantity = 2,
                            Name = "LG 1755",
                            Price = 12000.75m
                        }
                    }
                },
                "Номер чека не может быть пустым! \"/orders/order/no\" \n Проверьте загружаемый XML-файл"
            };

            // Missing Registration Date
            yield return new object[]
            {
                new XmlOrder
                {
                    No = 1,
                    // RegDate not set
                    User = new XmlUser
                    {
                        Fio = "Иванов Иван Иванович",
                        Email = "abc@email.com"
                    },
                    Products = new List<XmlProduct>
                    {
                        new XmlProduct
                        {
                            Quantity = 2,
                            Name = "LG 1755",
                            Price = 12000.75m
                        }
                    }
                },
                "Дата чека не может быть пустой! \"/orders/order/reg_date\" \n Проверьте загружаемый XML-файл"
            };

            // Missing User FIO
            yield return new object[]
            {
                new XmlOrder
                {
                    No = 1,
                    RegDate = DateOnly.Parse("2012-12-19"),
                    User = new XmlUser
                    {
                        Fio = "",
                        Email = "abc@email.com"
                    },
                    Products = new List<XmlProduct>
                    {
                        new XmlProduct
                        {
                            Quantity = 2,
                            Name = "LG 1755",
                            Price = 12000.75m
                        }
                    }
                },
                "Поле ФИО должно быть заполнено! \"orders/order/fio\" \n Проверьте загружаемый XML-файл"
            };

            // Missing User Email
            yield return new object[]
            {
                new XmlOrder
                {
                    No = 1,
                    RegDate = DateOnly.Parse("2012-12-19"),
                    User = new XmlUser
                    {
                        Fio = "Иванов Иван Иванович",
                        Email = ""
                    },
                    Products = new List<XmlProduct>
                    {
                        new XmlProduct
                        {
                            Quantity = 2,
                            Name = "LG 1755",
                            Price = 12000.75m
                        }
                    }
                },
                "Поле ФИО должно быть заполнено! \"orders/order/email\" \n Проверьте загружаемый XML-файл"
            };

            // Product with negative Quantity
            yield return new object[]
            {
                new XmlOrder
                {
                    No = 1,
                    RegDate = DateOnly.Parse("2012-12-19"),
                    User = new XmlUser
                    {
                        Fio = "Иванов Иван Иванович",
                        Email = "abc@email.com"
                    },
                    Products = new List<XmlProduct>
                    {
                        new XmlProduct
                        {
                            Quantity = -1,
                            Name = "LG 1755",
                            Price = 12000.75m
                        }
                    }
                },
                "Количество товара должно быть больше нуля! \"/orders/order/quantity\" \n Проверьте загружаемый XML-файл"
            };

            // Product with empty Name
            yield return new object[]
            {
                new XmlOrder
                {
                    No = 1,
                    RegDate = DateOnly.Parse("2012-12-19"),
                    User = new XmlUser
                    {
                        Fio = "Иванов Иван Иванович",
                        Email = "abc@email.com"
                    },
                    Products = new List<XmlProduct>
                    {
                        new XmlProduct
                        {
                            Quantity = 2,
                            Name = "",
                            Price = 12000.75m
                        }
                    }
                },
                "Имя товара обязательно должно быть заполнено! \"/orders/order/name\" \n Проверьте загружаемый XML-файл"
            };

            // Product with non-positive Price
            yield return new object[]
            {
                new XmlOrder
                {
                    No = 1,
                    RegDate = DateOnly.Parse("2012-12-19"),
                    User = new XmlUser
                    {
                        Fio = "Иванов Иван Иванович",
                        Email = "abc@email.com"
                    },
                    Products = new List<XmlProduct>
                    {
                        new XmlProduct
                        {
                            Quantity = 2,
                            Name = "LG 1755",
                            Price = 0m
                        }
                    }
                },
                "Цена товара должна быть больше нуля! \"/orders/order/price\" \n Проверьте загружаемый XML-файл"
            };
        }

    }
}
