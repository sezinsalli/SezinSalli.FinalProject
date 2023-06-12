using Simpra.Schema.Base;
using Simpra.Schema.ProductRR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Schema.CategoryRR
{
    public class CategoryResponse:BaseResponse
    {

        public string Name { get; set; }
        public string Url { get; set; }
        public string Tag { get; set; }
        public ICollection<ProductResponse> Products { get; set; }
    }
}
