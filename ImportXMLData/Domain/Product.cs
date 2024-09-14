using System.ComponentModel.DataAnnotations;

namespace ImportXMLData.Domain
{
    public class Product
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(255)]
        public required string Name { get; set; }
        public required string Description { get; set; } // nvarchar(max) - по умолчанию для EF
        public decimal Price { get; set; }
        public ICollection<PurchaseDetail>? PurchaseDetails { get; set; }
    }
}
