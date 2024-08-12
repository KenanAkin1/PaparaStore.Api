using PaparaStore.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaparaStore.Data.Domain;
[Table("Wallet", Schema = "dbo")]

public class Wallet : BaseEntity
{
    public long UserId { get; set; }
    public virtual User User { get; set; }

    public decimal Balance { get; set; }
    public decimal RewardPoints { get; set; }
    public bool IsActive { get; set; }

    public virtual List<WalletTransaction> WalletTransactions { get; set; }
}
