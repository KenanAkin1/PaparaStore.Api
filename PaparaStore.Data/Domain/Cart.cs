using PaparaStore.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaparaStore.Data.Domain
{
    [Table("Cart", Schema = "dbo")]
    public class Cart : BaseEntity
    {
        public long UserId { get; set; }
        public int ProductAmount { get; set; }
        public bool IsActive { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalRewardPoints { get; set; }

        public virtual ICollection<CartProduct> CartProducts { get; set; }
        public virtual User User { get; set; }
    }
}
