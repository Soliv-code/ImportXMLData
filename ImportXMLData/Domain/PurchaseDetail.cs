using System.ComponentModel.DataAnnotations;

namespace ImportXMLData.Domain
{
    public class PurchaseDetail
    {
        [Key]
        public long Id { get; set; }
        public required long UserPurchaseId { get; set; }
        public required UserPurchase UserPurchase { get; set; }
        public required long ProductId { get; set; }
        public required Product Product { get; set; }
        public required int Qnt { get; set; }
    }
}
