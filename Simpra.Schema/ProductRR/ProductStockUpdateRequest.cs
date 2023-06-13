using Simpra.Schema.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Schema.ProductRR
{
    public class ProductStockUpdateRequest:BaseRequest
    {
        public int Id { get; set; } 
        
        // 3 ürün geldiyse 3 yazacak - 2 ürün çıktıysa 2 yazacak client
        public int StockChange { get; set; }
    }
}
