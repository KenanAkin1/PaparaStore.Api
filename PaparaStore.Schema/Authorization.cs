using PaparaStore.Base.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaparaStore.Schema;
public class AuthorizationRequest : BaseRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}


public class AuthorizationResponse : BaseResponse
{
    public DateTime ExpireTime { get; set; }
    public string AccessToken { get; set; }
    public string Email { get; set; }
}