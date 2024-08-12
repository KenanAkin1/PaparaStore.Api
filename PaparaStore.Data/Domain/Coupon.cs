using PaparaStore.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaparaStore.Data.Domain;
[Table("Coupon", Schema = "dbo")]
public class Coupon : BaseEntity
{
    public string Code { get; set; }
    public decimal? MinValue { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTime EndDate { get; set; }
    public long CouponQuantity { get; set; }
    public bool IsActive { get; set; }

    public virtual ICollection<Order> Orders { get; set; }

}