using PaparaStore.Base.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PaparaStore.Schema;
public class OrderRequest : BaseRequest
{
    [JsonIgnore]
    public long UserId { get; set; }
    public string? CouponCode { get; set; }
    public PaymentRequest? PaymentRequest { get; set; }

}


public class OrderResponse
{
    public long OrderNumber { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal FinalPrice { get; set; }
    public decimal TotalRewardAmount { get; set; } 
    public decimal WalletBalanceUsed { get; set; }
    public decimal RewardPointsUsed { get; set; } 
    public ICollection<OrderDetailResponse> OrderDetails { get; set; }
}
