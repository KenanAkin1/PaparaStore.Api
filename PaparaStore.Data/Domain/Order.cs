using PaparaStore.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaparaStore.Data.Domain;
[Table("Order", Schema = "dbo")]
public class Order : BaseEntity
{
    public long UserId { get; set; }
    public virtual User User { get; set; }

    public string CouponCode { get; set; }
    public virtual Coupon Coupon { get; set; }


    public long OrderNumber { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal FinalPrice { get; set; }
    public decimal TotalRewardAmount { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; }

}
