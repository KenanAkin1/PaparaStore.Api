using PaparaStore.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaparaStore.Data.Domain;

[Table("CategoryProduct", Schema = "dbo")]
public class CategoryProduct : BaseEntity
{
    public long CategoryId { get; set; }
    public Category Category { get; set; }

    public long ProductId { get; set; }
    public Product Product { get; set; }
}
