using ImportXMLData.Data;
using ImportXMLData.Deserializer;
using ImportXMLData.Map;
using ImportXMLData.Validator;
using System.Xml;

public class Program
{
    private static readonly IXmlDeserializer _deserializer = new XmlDeserializer();
    private static readonly IDataValidator _validator = new DataValidator();
    private static readonly IMapper _mapper = new Mapper(new TestTaskSkrinDbContext()); // Inject DbContext
    public static async Task Main(string[] args)
    {
        string xmlFilePath;
        while (true)
        {
            Console.WriteLine("Укажите полный путь к файлу (например: c:\\import.xml");
            xmlFilePath = Console.ReadLine() ?? string.Empty;
            if (File.Exists(xmlFilePath))
                break;
            Console.WriteLine($"Файл не найден по пути: {xmlFilePath}");
            Console.WriteLine();
        }
        try
        {

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(xmlFilePath);
                XmlNodeList? orders = xmlDoc.SelectNodes("/orders/order");
                if (orders is null)
                    throw new ArgumentException("Не удалось найти обязательные элементы xml-файла: \"/orders/orders\"");

                var _orders = _deserializer.Deserialize(xmlFilePath);
                _validator.Validate(_orders);
                await _mapper.Map(_orders);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка обработки XML-файла: " + ex.Message);
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Возникла ошибка при обработке XML-файла: {ex.Message}"); ;
        };

        Console.WriteLine("Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}
