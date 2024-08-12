using PaparaStore.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaparaStore.Data.Domain;

[Table("Category", Schema = "dbo")]
public class Category : BaseEntity
{
    public string Name { get; set; }
    public virtual List<string> Tags { get; set; } = new List<string>();
    public string ImageUrl { get; set; }
    public string Url { get; set; }
    public string? Description { get; set; }

    public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
}
