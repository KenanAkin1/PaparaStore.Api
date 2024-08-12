using PaparaStore.Base.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaparaStore.Data.Domain;

[Table("Product", Schema = "dbo")]
public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public decimal LastPrice { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int RewardPercantage { get; set; }
    public decimal MaxRewardAmount { get; set; }
    public bool IsActive { get; set; }

    public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
    public virtual ICollection<CartProduct> CartProducts { get; set; }
    public virtual ICollection<ProductCode> ProductCodes { get; set; }
}
