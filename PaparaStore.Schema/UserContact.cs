using PaparaStore.Base.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaparaStore.Schema;
public class UserContactRequest : BaseRequest
{
    public string CountryCode { get; set; }
    public string Phone { get; set; }
}
public class UserContactResponse : BaseResponse
{
    public string UserId { get; set; }
    public string CountryCode { get; set; }
    public string Phone { get; set; }

    public string UserName { get; set; }
}
