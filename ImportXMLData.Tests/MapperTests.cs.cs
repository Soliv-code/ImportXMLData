using ImportXMLData.Map;
using ImportXMLData.Data;
using ImportXMLData.Domain.XmlDeserializerDomain;
using ImportXMLData.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace ImportXMLData.Tests
{
    public class MapperTests
    {
        private readonly DbContextOptions<TestTaskSkrinDbContext> _dbOptions;

        public MapperTests()
        {
            // Configure in-memory database
            _dbOptions = new DbContextOptionsBuilder<TestTaskSkrinDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task Map_ValidOrders_InsertsData()
        {
            // Arrange
            using (var context = new TestTaskSkrinDbContext(_dbOptions))
            {
                var mapper = new Mapper(context);

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
                            },
                            new XmlProduct
                            {
                                Quantity = 5,
                                Name = "Xiomi 12X",
                                Price = 42000.75m
                            }
                        }
                    }
                };

                // Act
                await mapper.Map(orders);

                // Assert
                var users = await context.Users.ToListAsync();
                users.Should().HaveCount(1);
                var user = users.First();
                user.Name.Should().Be("Иванов Иван Иванович");
                user.Email.Should().Be("abc@email.com");

                var userPurchases = await context.UserPurchases.ToListAsync();
                userPurchases.Should().HaveCount(1);
                var purchase = userPurchases.First();
                purchase.Number.Should().Be("1");
                purchase.PurchaseDate.Should().Be(DateOnly.Parse("2012-12-19"));
                purchase.UserId.Should().Be(user.Id);

                var products = await context.Products.ToListAsync();
                products.Should().HaveCount(2);
                products.Should().Contain(p => p.Name == "LG 1755" && p.Price == 12000.75m);
                products.Should().Contain(p => p.Name == "Xiomi 12X" && p.Price == 42000.75m);

                var purchaseDetails = await context.PurchaseDetails.ToListAsync();
                purchaseDetails.Should().HaveCount(2);
                purchaseDetails.Should().Contain(pd => pd.Product.Name == "LG 1755" && pd.Qnt == 2 && pd.UserPurchaseId == purchase.Id);
                purchaseDetails.Should().Contain(pd => pd.Product.Name == "Xiomi 12X" && pd.Qnt == 5 && pd.UserPurchaseId == purchase.Id);
            }
        }

        [Fact]
        public async Task Map_DuplicateOrder_SkipsInsertion()
        {
            // Arrange
            using (var context = new TestTaskSkrinDbContext(_dbOptions))
            {
                // Seed existing order
                var existingUser = new User
                {
                    Id = 1,
                    Name = "Иванов Иван Иванович",
                    Email = "abc@email.com",
                    Address = ""
                };
                context.Users.Add(existingUser);

                var existingPurchase = new UserPurchase
                {
                    Id = 1,
                    UserId = existingUser.Id,
                    Number = "1",
                    PurchaseDate = DateOnly.Parse("2012-12-19")
                };
                context.UserPurchases.Add(existingPurchase);
                await context.SaveChangesAsync();

                var mapper = new Mapper(context);

                var orders = new List<XmlOrder>
                {
                    new XmlOrder
                    {
                        No = 1, // Duplicate order number
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
                await mapper.Map(orders);

                // Assert
                var userPurchases = await context.UserPurchases.ToListAsync();
                userPurchases.Should().HaveCount(1); // No new purchase added
            }
        }

        [Fact]
        public async Task Map_NewUser_AddsUser()
        {
            // Arrange
            using (var context = new TestTaskSkrinDbContext(_dbOptions))
            {
                var mapper = new Mapper(context);

                var orders = new List<XmlOrder>
                {
                    new XmlOrder
                    {
                        No = 2,
                        RegDate = DateOnly.Parse("2021-05-10"),
                        User = new XmlUser
                        {
                            Fio = "Петров Пётр Петрович",
                            Email = "petrov@example.com"
                        },
                        Products = new List<XmlProduct>
                        {
                            new XmlProduct
                            {
                                Quantity = 1,
                                Name = "Noname 14232",
                                Price = 1.7m
                            }
                        }
                    }
                };

                // Act
                await mapper.Map(orders);

                // Assert
                var users = await context.Users.ToListAsync();
                users.Should().HaveCount(1);
                var user = users.First();
                user.Name.Should().Be("Петров Пётр Петрович");
                user.Email.Should().Be("petrov@example.com");
            }
        }

        [Fact]
        public async Task Map_ExistingProduct_UsesExistingProduct()
        {
            // Arrange
            using (var context = new TestTaskSkrinDbContext(_dbOptions))
            {
                // Seed existing product
                var existingProduct = new Product
                {
                    Id = 1,
                    Name = "LG 1755",
                    Description = "",
                    Price = 12000.75m
                };
                context.Products.Add(existingProduct);
                await context.SaveChangesAsync();

                var mapper = new Mapper(context);

                var orders = new List<XmlOrder>
                {
                    new XmlOrder
                    {
                        No = 3,
                        RegDate = DateOnly.Parse("2023-01-01"),
                        User = new XmlUser
                        {
                            Fio = "Сидоров Сидор Сидорович",
                            Email = "sidor@example.com"
                        },
                        Products = new List<XmlProduct>
                        {
                            new XmlProduct
                            {
                                Quantity = 3,
                                Name = "LG 1755", // Existing product
                                Price = 12000.75m
                            },
                            new XmlProduct
                            {
                                Quantity = 4,
                                Name = "New Product",
                                Price = 5000m
                            }
                        }
                    }
                };

                // Act
                await mapper.Map(orders);

                // Assert
                var products = await context.Products.ToListAsync();
                products.Should().HaveCount(2); // Existing + New
                products.Should().Contain(p => p.Name == "LG 1755");
                products.Should().Contain(p => p.Name == "New Product");

                var purchaseDetails = await context.PurchaseDetails.ToListAsync();
                purchaseDetails.Should().HaveCount(2);
                var existingPd = purchaseDetails.FirstOrDefault(pd => pd.Product.Name == "LG 1755");
                existingPd.Should().NotBeNull();
                existingPd.Qnt.Should().Be(3);

                var newPd = purchaseDetails.FirstOrDefault(pd => pd.Product.Name == "New Product");
                newPd.Should().NotBeNull();
                newPd.Qnt.Should().Be(4);
            }
        }
    }
}
