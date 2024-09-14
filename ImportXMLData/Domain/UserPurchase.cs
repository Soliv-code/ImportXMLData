using System.ComponentModel.DataAnnotations;

namespace ImportXMLData.Domain
{
    public class UserPurchase
    {
        [Key]
        public long Id { get; set; }
        public required long UserId { get; set; }
        public required DateOnly PurchaseDate { get; set; }
        public required string Number { get; set; }
        public ICollection<PurchaseDetail>? PurchaseDetails { get; set; }
    }
}
