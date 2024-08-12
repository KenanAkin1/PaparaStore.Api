
namespace PaparaStore.Schema;
public class PaymentRequest
{
    public string CardNumber { get; set; }
    public string ExpiryDate { get; set; }
    public string CVV { get; set; }
    public string CardHolderName { get; set; }
}
