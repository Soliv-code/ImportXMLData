using System.ComponentModel.DataAnnotations;

namespace ImportXMLData.Domain
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(255)]
        public required string Name { get; set; }
        [MaxLength(255)]
        public required string Email { get; set; }
        public required string Address { get; set; }
        public ICollection<UserPurchase>? UserPurchases { get; set; }
    }
}
