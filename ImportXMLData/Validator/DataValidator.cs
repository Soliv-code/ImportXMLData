using ImportXMLData.Domain.XmlDeserializerDomain;
using Microsoft.IdentityModel.Tokens;

namespace ImportXMLData.Validator
{
    public class DataValidator
    {
        private readonly string _lineBreakCheckFile = " \n Проверьте загружаемый XML-файл";
        public void Validate(List<XmlOrder> orders)
        {
            foreach (var order in orders)
            {
                ValidateOrder(order);
                ValidateProducts(order.Products);
                ValidateUser(order.User);
            }
        }

        private void ValidateOrder(XmlOrder order)
        {
            if (order.No.ToString().IsNullOrEmpty())
                throw new ArgumentException("Номер чека не может быть пустым! \"/orders/order/no\"" + _lineBreakCheckFile);
            if (order.RegDate.ToString().IsNullOrEmpty())
                throw new ArgumentException("Дата чека не может быть пустой! \"/orders/order/reg_date\"" + _lineBreakCheckFile);
        }
        private void ValidateProducts(List<XmlProduct> products)
        {
            foreach (var product in products)
            {
                if (product.Quantity <= 0)
                    throw new ArgumentException("Количество товара должно быть больше нуля! \"/orders/order/quantity\"" + _lineBreakCheckFile);
                if (string.IsNullOrEmpty(product.Name))
                    throw new ArgumentException("Имя товара обязательно должно быть заполнено! \"/orders/order/name\"" + _lineBreakCheckFile);
                if (product.Price <= 0)
                    throw new ArgumentException("Цена товара должна быть больше нуля! \"/orders/order/price\"" + _lineBreakCheckFile);
            }
        }
        private void ValidateUser(XmlUser user)
        {
            if (string.IsNullOrEmpty(user.Fio))
                throw new ArgumentException("Поле ФИО должно быть заполнено! \"orders/order/fio\"" + _lineBreakCheckFile);
            if(string.IsNullOrEmpty(user.Email))
                throw new ArgumentException("Поле ФИО должно быть заполнено! \"orders/order/email\"" + _lineBreakCheckFile);
        }
    }
}
