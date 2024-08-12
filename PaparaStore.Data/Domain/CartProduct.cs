using PaparaStore.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaparaStore.Data.Domain
{
    [Table("CartProduct", Schema = "dbo")]
    public class CartProduct : BaseEntity
    {
        public long CartId { get; set; }
        public virtual Cart Cart { get; set; }

        public long ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal RewardAmount { get; set; }

        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}
