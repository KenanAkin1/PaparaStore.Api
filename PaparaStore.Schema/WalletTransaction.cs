using PaparaStore.Base.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaparaStore.Schema;
public class WalletTransactionRequest : BaseRequest
{
    public long WalletId { get; set; }
    public long ReferenceNumber { get; set; }
    public decimal InitialAmount { get; set; }
    public decimal SpentAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public string? Description { get; set; }
}


public class WalletTransactionResponse : BaseResponse
{
    public long WalletId { get; set; }
    public long ReferenceNumber { get; set; }
    public decimal InitialAmount { get; set; }
    public decimal SpentAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public string? Description { get; set; }
    public DateTime TransactionDate { get; set; }

    public string UserName { get; set; }
}