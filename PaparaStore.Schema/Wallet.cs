using PaparaStore.Base.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PaparaStore.Schema;

public class WalletRequest : BaseRequest
{
    public long UserId { get; set; }
    public double Balance { get; set; }
    public double RewardPoints { get; set; }
}
public class WalletResponse : BaseResponse
{
    public long UserId { get; set; }
    public string UserName { get; set; }
    public double Balance { get; set; }
    public double RewardPoints { get; set; }
}