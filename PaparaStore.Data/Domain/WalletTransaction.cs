using PaparaStore.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;
namespace PaparaStore.Data.Domain;

[Table("WalletTransaction", Schema = "dbo")]
public class WalletTransaction : BaseEntity
{
    public long WalletId { get; set; }
    public virtual Wallet Wallet { get; set; }

    public long ReferenceNumber { get; set; }
    public decimal InitialAmount { get; set; }
    public decimal SpentAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public string? Description { get; set; }
    public DateTime TransactionDate { get; set; }
}
