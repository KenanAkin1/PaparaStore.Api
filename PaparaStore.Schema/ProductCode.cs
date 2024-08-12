using PaparaStore.Base.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaparaStore.Schema;
public class ProductCodeRequest : BaseRequest
{
    public long ProductId { get; set; }
    
    public string Code { get; set; }
}


public class ProductCodeResponse : BaseResponse
{
    public long ProductId { get; set; }

    public string Code { get; set; }
}
