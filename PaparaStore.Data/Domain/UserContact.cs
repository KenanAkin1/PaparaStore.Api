using PaparaStore.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaparaStore.Data.Domain;
[Table("UserContact", Schema = "dbo")]

public class UserContact : BaseEntity
{
    public long UserId { get; set; }
    virtual public User User { get; set; }

    public string CountryCode { get; set; }
    public string Phone { get; set; }
    public bool IsDefault { get; set; }
}
