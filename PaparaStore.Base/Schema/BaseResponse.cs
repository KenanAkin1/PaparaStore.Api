namespace PaparaStore.Base.Schema;

public abstract class BaseResponse
{
    public DateTime ServerDate { get; set; } = DateTime.UtcNow;
    public long Id { get; set; }
    //public string CustomerNumber { get; set; }
}
