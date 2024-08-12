using PaparaStore.Base.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaparaStore.Schema;
public class CategoryProductRequest : BaseRequest
{
    public long CategoryId { get; set; }
    public long ProductId { get; set; }

}
public class CategoryProductResponse : BaseResponse
{
    public long CategoryId { get; set; }
    public long ProductId { get; set; }
}