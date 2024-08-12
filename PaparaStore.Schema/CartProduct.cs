using PaparaStore.Base.Schema;

namespace PaparaStore.Schema;
public class CartProductRequest : BaseRequest
{
    public long CartId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
}
public class CartProductResponse : BaseResponse
{
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal RewardAmount { get; set; }
    public string ProductName { get; set; }
    public decimal ProductPrice { get; set; }
    public string ProductImageUrl { get; set; }
}