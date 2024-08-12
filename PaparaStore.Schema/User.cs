using PaparaStore.Base.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaparaStore.Schema;
public class UserRequest : BaseRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

}


public class UserResponse : BaseResponse
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public int Status { get; set; }

    public WalletResponse Wallet { get; set; }
    public List<UserContactResponse> UserContacts { get; set; }

}
