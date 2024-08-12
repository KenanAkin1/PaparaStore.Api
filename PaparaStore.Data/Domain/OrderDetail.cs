using PaparaStore.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaparaStore.Data.Domain;
[Table("OrderDetail", Schema = "dbo")]
public class OrderDetail : BaseEntity
{
    public long OrderId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal RewardAmount { get; set; }

    public virtual Order Order { get; set; }
    public virtual Product Product { get; set; }
}