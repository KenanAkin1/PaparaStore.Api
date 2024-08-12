using PaparaStore.Base.Schema;


namespace PaparaStore.Schema;
public class CartRequest : BaseRequest
{
    public long UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<CartProductRequest> CartProducts { get; set; }
}
public class CartResponse
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public int ProductAmount { get; set; }
    public List<CartProductResponse> CartProducts { get; set; }
}
