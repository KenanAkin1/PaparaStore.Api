using PaparaStore.Base.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaparaStore.Schema;
public class ProductRequest : BaseRequest
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int RewardPercantage { get; set; }
    public decimal MaxRewardAmount { get; set; }

    public List<string> ProductCodes { get; set; }
    public List<long> CategoryIds { get; set; }
}


public class ProductResponse : BaseResponse
{
    public List<long> CategoryIds { get; set; }

    public string Name { get; set; }
    public string? Description { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int RewardPercantage { get; set; }
    public decimal MaxRewardAmount { get; set; }

    public List<string> CategoryNames { get; set; }
    public List<string> CategoryTags { get; set; }
}
