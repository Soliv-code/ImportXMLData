using ImportXMLData.Data;
using ImportXMLData.Domain;
using ImportXMLData.Domain.XmlDeserializerDomain;
using Microsoft.EntityFrameworkCore;

namespace ImportXMLData.Map
{
    public class Mapper
    {
        public async Task Map(List<XmlOrder> orders)
        {
            await using (TestTaskSkrinDbContext _db = new TestTaskSkrinDbContext())
            {
                foreach (var order in orders)
                {
                    User? _existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Name == order.User.Fio && u.Email == order.User.Email);
                    if (_existingUser is null)
                    {
                        _existingUser = new User
                        {
                            Name = order.User.Fio,
                            Email = order.User.Email,
                            Address = "",
                            UserPurchases = null
                        };
                    }
                    await _db.Users.AddAsync(_existingUser);
                    await _db.SaveChangesAsync();

                    UserPurchase purchaseEntity = new UserPurchase
                    {
                        UserId = _existingUser.Id,
                        PurchaseDate = order.RegDate,
                        Number = order.No.ToString(),
                        PurchaseDetails = null
                    };
                    await _db.UserPurchases.AddAsync(purchaseEntity);
                    await _db.SaveChangesAsync();


                    foreach (var product in order.Products)
                    {
                        Product? _existingProduct = await _db.Products.FirstOrDefaultAsync(p => p.Name == product.Name);
                        if (_existingProduct is null)
                        {
                            _existingProduct = new Product
                            {
                                Name = product.Name,
                                Description = "",
                                Price = product.Price,
                                PurchaseDetails = null
                            };
                            await _db.Products.AddAsync(_existingProduct);
                            await _db.SaveChangesAsync();
                        };

                        PurchaseDetail detailEntity = new PurchaseDetail
                        {
                            UserPurchaseId = purchaseEntity.Id,
                            UserPurchase = purchaseEntity,
                            ProductId = _existingProduct.Id,
                            Product = _existingProduct,
                            Qnt = product.Quantity,
                        };
                        await _db.PurchaseDetails.AddAsync(detailEntity);
                        await _db.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
