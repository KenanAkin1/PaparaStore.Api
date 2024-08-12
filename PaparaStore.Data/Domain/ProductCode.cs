using PaparaStore.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaparaStore.Data.Domain;
[Table("ProductCode", Schema = "dbo")]
public class ProductCode : BaseEntity
{
    public string Code { get; set; }

    public long ProductId { get; set; }
    public Product Product { get; set; }


}
