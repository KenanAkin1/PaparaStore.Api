using PaparaStore.Base.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaparaStore.Schema;
public class OrderDetailRequest : BaseRequest
{

    public long OrderNumber { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}


public class OrderDetailResponse : BaseResponse
{
    public long ProductId { get; set; }
    public long OrderNumber { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal RewardAmount { get; set; }
}
