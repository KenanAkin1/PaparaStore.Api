using PaparaStore.Base.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaparaStore.Schema;
public class CouponRequest : BaseRequest
{
    public string Code { get; set; }
    public decimal MinValue { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTime EndDate { get; set; }
    public long CouponQuantity { get; set; }
}


public class CouponResponse : BaseResponse
{
    public string Code { get; set; }
    public decimal MinValue { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTime EndDate { get; set; }
    public long CouponQuantity { get; set; }
}
