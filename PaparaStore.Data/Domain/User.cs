using PaparaStore.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaparaStore.Data.Domain;
[Table("User", Schema = "dbo")]
public class User : BaseEntity
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public int Status { get; set; }

    public virtual List<UserContact> UserContacts { get; set; }
    public virtual Wallet Wallet { get; set; }
    public virtual Cart Cart { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
}